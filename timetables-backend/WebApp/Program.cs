using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using App.BLL;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApp;
using WebApp.Helpers;
using AutoMapperProfile = WebApp.Helpers.AutoMapperProfile;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();

builder.Services.AddDbContext<AppDbContext>(options => { options.UseNpgsql(connectionString); });

builder.Services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
builder.Services.AddScoped<IAppBLL, AppBll>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
    .AddDefaultUI()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//clear default claims
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication()
    .AddCookie(options => { options.SlidingExpiration = true; })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = builder.Configuration.GetValue<string>("JWT:issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("JWT:audience"),
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:key"))),
            ClockSkew = TimeSpan.Zero,
        };
    });


builder.Services.AddControllersWithViews(options =>
    {
        options.ModelBinderProviders.Insert(0, new CustomLangStrBinderProvider());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.AllowTrailingCommas = true;
    });


var supportedCultures = builder.Configuration
    .GetSection("SupportedCultures")
    .GetChildren()
    .Select(x => new CultureInfo(x.Value))
    .ToArray();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    // datetime and currency support
    options.SupportedCultures = supportedCultures;
    // UI translated strings
    options.SupportedUICultures = supportedCultures;
    // if nothing is found, use this
    options.DefaultRequestCulture =
        new RequestCulture(
            builder.Configuration["DefaultCulture"],
            builder.Configuration["DefaultCulture"]);
    options.SetDefaultCulture(builder.Configuration["DefaultCulture"]);

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        // Order is important, its in which order they will be evaluated
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsAllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});


// reference any class from class library to be scanned for mapper configurations
builder.Services.AddAutoMapper(
    typeof(App.DAL.EF.AutoMapperProfile),
    typeof(App.BLL.AutoMapperProfile),
    typeof(AutoMapperProfile)
);


var apiVersioningBuilder = builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

apiVersioningBuilder.AddApiExplorer(options =>
{
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureModelBindingLocalization>();

//====================================================================
var app = builder.Build();
//====================================================================

//SetupAppData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("CorsAllowAll");

app.UseRequestLocalization(options:
    app.Services.GetService<IOptions<RequestLocalizationOptions>>()?.Value!);

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant()
        );
    }
    // serve from root
    // options.RoutePrefix = string.Empty;
});
app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contests}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();


static async void SetupAppData(WebApplication webApplication)
{
    
    using var serviceScope = ((IApplicationBuilder)webApplication).ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();
    using var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!context.Database.ProviderName!.Contains("InMemory"))
    {
        context.Database.Migrate();
    }


    using var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    using var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    
    if (roleManager.Roles.Any()) return;
    
    await roleManager.CreateAsync(new AppRole() { Name = "Contest Admin" });
    await roleManager.CreateAsync(new AppRole() { Name = "User" });
    await roleManager.CreateAsync(new AppRole() { Name = "Participant" });
    
    var user = new AppUser()
        { Email = "admin@eesti.ee", UserName = "admin@eesti.ee", FirstName = "ContestAdmin", LastName = "Eesti" };
    
    var randomUser = new AppUser()
        { Email = "random@gmail.com", UserName = "random@gmail.com", FirstName = "Random", LastName = "Eesti" };
    await userManager.CreateAsync(user, "Mina!1");
    await userManager.AddToRoleAsync(user, "Contest Admin");
    await userManager.CreateAsync(randomUser, "Uus!12");
    await userManager.AddToRoleAsync(randomUser, "User");

    var contestType = await context.ContestTypes.FirstOrDefaultAsync();
    if (contestType == null)
    {
        context.ContestTypes.Add(new ContestType()
            { ContestTypeName = "Treening", Description = "Trenn", AppUserId = user.Id, AppUser = user });
        context.ContestTypes.Add(new ContestType()
            { ContestTypeName = "Turniir", Description = "Turniir", AppUserId = user.Id, AppUser = user });
    }

    var gameTypes = await context.GameTypes.FirstOrDefaultAsync();
    if (gameTypes == null)
    {
        context.GameTypes.Add(new GameType() { GameTypeName = "Tennis", AppUserId = user.Id, AppUser = user });
        context.GameTypes.Add(new GameType() { GameTypeName = "Rannatennis", AppUserId = user.Id, AppUser = user });
        context.GameTypes.Add(new GameType() { GameTypeName = "Paddle", AppUserId = user.Id, AppUser = user });
    }

    var levels = await context.Levels.FirstOrDefaultAsync();
    if (levels == null)
    {
        context.Levels.Add(new Level() { Title = "A", Description = "Osav", AppUserId = user.Id, AppUser = user });
        context.Levels.Add(new Level()
            { Title = "B", Description = "Mitte nii osav", AppUserId = user.Id, AppUser = user });
        context.Levels.Add(new Level()
            { Title = "C", Description = "Harrastaja", AppUserId = user.Id, AppUser = user });
        context.Levels.Add(new Level() { Title = "D", Description = "Nõrk", AppUserId = user.Id, AppUser = user });
    }

    var timeOfDays = await context.TimeOfDays.FirstOrDefaultAsync();
    if (timeOfDays == null)
    {
        context.TimeOfDays.Add(new TimeOfDay() { TimeOfDayName = "Hommikune", AppUserId = user.Id, AppUser = user });
        context.TimeOfDays.Add(new TimeOfDay() { TimeOfDayName = "Lõunane", AppUserId = user.Id, AppUser = user });
        context.TimeOfDays.Add(new TimeOfDay() { TimeOfDayName = "Õhtune", AppUserId = user.Id, AppUser = user });
    }

    await context.SaveChangesAsync();

    var times = await context.Times.FirstOrDefaultAsync();
    if (times == null)
    {
        context.Times.Add(new Time()
        {
            TimeOfDay = context.TimeOfDays.ToList().Where(t => t.TimeOfDayName == "Hommikune").ToList().First(),
            From = new TimeOnly(10, 00),
            Until = new TimeOnly(12, 30),
            AppUserId = user.Id,
            AppUser = user
        });
        context.Times.Add(new Time()
        {
            TimeOfDay = context.TimeOfDays.ToList().Where(t => t.TimeOfDayName == "Lõunane").ToList().First(),
            From = new TimeOnly(13, 30),
            Until = new TimeOnly(15, 00),
            AppUserId = user.Id,
            AppUser = user
        });
        context.Times.Add(new Time()
        {
            TimeOfDay = context.TimeOfDays.ToList().Where(t => t.TimeOfDayName == "Õhtune").ToList().First(),
            From = new TimeOnly(17, 00),
            Until = new TimeOnly(19, 30),
            AppUserId = user.Id,
            AppUser = user
        });
    }

    var packageGameTypeTime = await context.PackageGameTypeTimes.FirstOrDefaultAsync();
    if (packageGameTypeTime == null)
    {
        context.PackageGameTypeTimes.Add(new PackageGameTypeTime()
        {
            PackageGtName = "Täielik paddle",
            GameType = context.GameTypes.ToList().Where(g => g.GameTypeName == "Paddle").ToList().First(),
            Times = 1,
            AppUserId = user.Id,
            AppUser = user
        });
        context.PackageGameTypeTimes.Add(new PackageGameTypeTime()
        {
            PackageGtName = "Poolik paddle",
            GameType = context.GameTypes.ToList().Where(g => g.GameTypeName == "Paddle").ToList().First(),
            Times = (decimal)0.5,
            AppUserId = user.Id,
            AppUser = user
        });
        context.PackageGameTypeTimes.Add(new PackageGameTypeTime()
        {
            PackageGtName = "Täielik tennis",
            GameType = context.GameTypes.ToList().Where(g => g.GameTypeName == "Tennis").ToList().First(),
            Times = (decimal)1,
            AppUserId = user.Id,
            AppUser = user
        });
        context.PackageGameTypeTimes.Add(new PackageGameTypeTime()
        {
            PackageGtName = "Poolik tennis",
            GameType = context.GameTypes.ToList().Where(g => g.GameTypeName == "Tennis").ToList()[0],
            Times = (decimal)0.5,
            AppUserId = user.Id,
            AppUser = user
        });
    }

    var location = await context.Locations.FirstOrDefaultAsync();
    if (location == null)
    {
        context.Locations.Add(new Location()
        {
            LocationName = "Laagri väljakud",
            State = "Harjumaa",
            Country = "Eesti",
            AppUserId = user.Id,
            AppUser = user
        });
    }
    await context.SaveChangesAsync();

    var contest = await context.Contests.FirstOrDefaultAsync();
    if (contest == null)
    {
        contest = context.Contests.Add(new Contest
        {
            Id = Guid.NewGuid(),
            ContestName = "Mai treening",
            Description = "Treening",
            From = DateTime.UtcNow.AddDays(2),
            Until = DateTime.UtcNow.AddDays(3),
            TotalHours = 12,
            ContestTypeId = context.ContestTypes
                .ToList()
                .FirstOrDefault(e => e.ContestTypeName == "Treening")!.Id,
            LocationId = context.Locations
                .ToList()
                .FirstOrDefault(e => e.LocationName == "Laagri väljakud")!.Id,
            AppUserId = user.Id,
        }).Entity;
        await context.SaveChangesAsync();
        
        var contestLevel = new ContestLevel
        {
            ContestId = contest!.Id,
            LevelId = context.Levels.ToList().First(e => e.Title == "A").Id
        };
        context.ContestLevels.Add(contestLevel);
        
        var contestGameType = new ContestGameType()
        {
            ContestId = contest!.Id,
            GameTypeId = context.GameTypes.ToList().First(e => e.GameTypeName == "Tennis").Id
        };
        context.ContestGameTypes.Add(contestGameType);

        foreach (var time in context.Times.ToList())
        {
            var timeSlot = new ContestTime()
            {
                ContestId = contest.Id,
                TimeId = time.Id
            };
            context.ContestTimes.Add(timeSlot);
        }
        
        var package = new ContestPackage()
        {
            ContestId = contest.Id,
            PackageGameTypeTimeId = context.PackageGameTypeTimes.ToList().First(e => e.PackageGtName == "Täielik tennis").Id,
        };
        context.ContestPackages.Add(package);
        
        //Contest Roles by default
        var trainerRole = new ContestRole
        {
            ContestRoleName = "Trainer",
            ContestId = contest.Id
        };

        var participantRole = new ContestRole()
        {
            ContestRoleName = "Participant",
            ContestId = contest.Id
        };
        context.ContestRoles.Add(participantRole);
        context.ContestRoles.Add(trainerRole);
    }

    await context.SaveChangesAsync();
    
    SetupClients(webApplication);
}

static async void SetupClients(WebApplication webApplication)
{
    using var serviceScope = ((IApplicationBuilder)webApplication).ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();
    using var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

    using var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    using var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

    var ALevel = context.Levels.ToList().First(e => e.Title == "A");
    var BLevel = context.Levels.ToList().First(e => e.Title == "B");
    var CLevel = context.Levels.ToList().First(e => e.Title == "C");
    var DLevel = context.Levels.ToList().First(e => e.Title == "D");
    var contest = context.Contests.ToList().Where(e => e.ContestName == "Mai treening").ToList()[0];

    foreach (var gameType in context.GameTypes.ToList())
    {
        var package = new PackageGameTypeTime();
        if (gameType.GameTypeName == "Paddle")
        {
            continue;
            package = await context.PackageGameTypeTimes.Where(e => e.PackageGtName == "Täielik paddle").FirstOrDefaultAsync();
        } else if (gameType.GameTypeName == "Tennis")
        {
            package = context.PackageGameTypeTimes.ToList().First(e => e.PackageGtName == "Täielik tennis");
        }
        else
        {
            //rannatennise
            continue;
        }
        foreach (var level in context.Levels.ToList().Where(e => e.Id == ALevel!.Id)
                     .ToList())
        {
            for (int i = 0; i < 8; i++)
            {
                var user = new AppUser()
                {
                    Email = "paddle" + i + "@eesti.ee", UserName = "uus" + i + "@eesti.ee", FirstName =  i.ToString(),
                    LastName = level.Title, Id = Guid.NewGuid()
                };
                await userManager.CreateAsync(user, "Uus!12");

                //Join Contest
                
                //Team
                var team = new Team() // Create team
                {
                    Id = Guid.NewGuid(),
                    TeamName = user.FirstName + " tiim",
                    LevelId = level.Id,
                    GameTypeId = gameType.Id,
                };
                context.Teams.Add(team);
                
                //UserPackage
                var userPackage = new UserContestPackage
                {
                    AppUser = user,
                    AppUserId = user.Id,
                    ContestId = contest!.Id,
                    TeamId = team.Id,
                    LevelId = level.Id,
                    PackageGameTypeTimeId = package!.Id,
                    HoursAvailable = 8,
                };
                context.UserContestPackages.Add(userPackage);
                await context.SaveChangesAsync();

                //Create all times for team
                var timeOfDays = context.TimeOfDays.ToList();
                var contestStartDate =
                    context.Contests.ToList()
                        .First(t => t.Id == contest.Id).From.Date;
                var contestEndDate =
                    context.Contests.ToList()
                        .First(t => t.Id == contest.Id).Until.Date;
                var contestLength = (contestEndDate - contestStartDate).TotalDays;

                for (int j = 0; j <= contestLength; j++)
                {
                    foreach (var timeOfDay in timeOfDays)
                    {
                        context.TimeTeams.Add(new TimeTeam
                        {
                            TimeOfDayId = timeOfDay.Id,
                            Day = DateOnly.FromDateTime(contestStartDate.AddDays(j)),
                            TeamId = team.Id,
                        });
                    }
                }
            }
        }
    }

    await context.SaveChangesAsync();
}


// for unit testing, to change auto generated top level statement class to public
public partial class Program
{
}
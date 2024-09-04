using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Test.Integration;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // change the di container registrations

            // find DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<AppDbContext>));

            // if found - remove
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            // and new DbContext
            services.AddDbContext<AppDbContext>(options => { options.UseSqlite(connection); });


            // create db and seed data
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<AppDbContext>();

            var logger = scopedServices
                .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

            context.Database.EnsureCreated();
            var userManager = scopedServices.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scopedServices.GetRequiredService<RoleManager<AppRole>>();

            SetupAppData(context, userManager, roleManager);
            SetupClients(context, userManager);
        });
    }

    static async void SetupAppData(AppDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        await roleManager.CreateAsync(new AppRole() { Name = "Contest Admin" });
        await roleManager.CreateAsync(new AppRole() { Name = "Treener" });
        await roleManager.CreateAsync(new AppRole() { Name = "Külaline" });
        await roleManager.CreateAsync(new AppRole() { Name = "Osavõtja" });
        
        var user = new AppUser()
            { Email = "admin@eesti.ee", UserName = "admin@eesti.ee", FirstName = "ContestAdmin", LastName = "Eesti" };

        await userManager.CreateAsync(user, "Mina!1");
        await userManager.AddToRoleAsync(user, "Contest Admin");

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
            context.TimeOfDays.Add(new TimeOfDay()
                { TimeOfDayName = "Hommikune", AppUserId = user.Id, AppUser = user });
            context.TimeOfDays.Add(new TimeOfDay() { TimeOfDayName = "Lõunane", AppUserId = user.Id, AppUser = user });
            context.TimeOfDays.Add(new TimeOfDay() { TimeOfDayName = "Õhtune", AppUserId = user.Id, AppUser = user });
        }

        await context.SaveChangesAsync();

        var times = await context.Times.FirstOrDefaultAsync();
        if (times == null)
        {
            context.Times.Add(new Time()
            {
                TimeOfDay = context.TimeOfDays.Where(t => t.TimeOfDayName == "Hommikune").ToList().First(),
                From = new TimeOnly(10, 00),
                Until = new TimeOnly(12, 30),
                AppUserId = user.Id,
                AppUser = user
            });
            context.Times.Add(new Time()
            {
                TimeOfDay = context.TimeOfDays.Where(t => t.TimeOfDayName == "Lõunane").ToList().First(),
                From = new TimeOnly(13, 30),
                Until = new TimeOnly(15, 00),
                AppUserId = user.Id,
                AppUser = user
            });
            context.Times.Add(new Time()
            {
                TimeOfDay = context.TimeOfDays.Where(t => t.TimeOfDayName == "Õhtune").ToList().First(),
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
                GameType = context.GameTypes.Where(g => g.GameTypeName == "Paddle").ToList().First(),
                Times = 1,
                AppUserId = user.Id,
                AppUser = user
            });
            context.PackageGameTypeTimes.Add(new PackageGameTypeTime()
            {
                PackageGtName = "Poolik paddle",
                GameType = context.GameTypes.Where(g => g.GameTypeName == "Paddle").ToList().First(),
                Times = (decimal)0.5,
                AppUserId = user.Id,
                AppUser = user
            });
            context.PackageGameTypeTimes.Add(new PackageGameTypeTime()
            {
                PackageGtName = "Täielik tennis",
                GameType = context.GameTypes.Where(g => g.GameTypeName == "Tennis").ToList().First(),
                Times = (decimal)1,
                AppUserId = user.Id,
                AppUser = user
            });
            context.PackageGameTypeTimes.Add(new PackageGameTypeTime()
            {
                PackageGtName = "Poolik tennis",
                GameType = context.GameTypes.Where(g => g.GameTypeName == "Tennis").ToList()[0],
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

        context.Courts.Add(new Court
        {
            CourtName = "Court 1",
            GameTypeId = context.GameTypes.First(e => e.GameTypeName == "Tennis").Id,
            LocationId = context.Locations.First(e => e.LocationName == "Laagri väljakud").Id,
            AppUserId = user.Id,
        });
        context.Courts.Add(new Court
        {
            CourtName = "Court 2",
            GameTypeId = context.GameTypes.First(e => e.GameTypeName == "Tennis").Id,
            LocationId = context.Locations.First(e => e.LocationName == "Laagri väljakud").Id,
            AppUserId = user.Id,
        });

        var contest = new Contest
        {
            ContestName = "New Contest",
            Description = "Something",
            TotalHours = 10,
            From = new DateTime(new DateOnly(2024,
                    05,
                    20),
                new TimeOnly(10,
                    00)),
            Until = new DateTime(new DateOnly(2024,
                    05,
                    24),
                new TimeOnly(22,
                    00)),
            ContestTypeId = context.ContestTypes.First(e => e.ContestTypeName == "Treening")
                .Id,
            LocationId = context.Locations.First()
                .Id,
            AppUserId = user.Id,
        };
        context.Contests.Add(contest);
        
        await context.SaveChangesAsync();
        
        //ContestGameTypes
        foreach (var gameType in context.GameTypes.ToList())
        {
            if (gameType.GameTypeName == "Tennis")
            {
                var contestGameTypes = new ContestGameType
                {
                    ContestId = contest.Id,
                    GameTypeId = gameType.Id,
                };
                context.ContestGameTypes.Add(contestGameTypes);

            }
        }

        //Contest Levels
        foreach (var level in context.Levels.ToList())
        {
            if (level.Title == "A" || level.Title =="B")
            {
                var contestLevel = new ContestLevel()
                {
                    ContestId = contest.Id,
                    LevelId = level.Id,
                };
                context.ContestLevels.Add(contestLevel);

            }
        }
        
        //Contest Levels
        foreach (var package in context.PackageGameTypeTimes.Where(e => e.GameType!.GameTypeName == "Tennis").ToList())
        {
            var contestPackage = new ContestPackage()
            {
                ContestId = contest.Id,
                PackageGameTypeTimeId = package.Id,
            };
            context.ContestPackages.Add(contestPackage);
                
        }
        
        //ContestTimes
        foreach (var time in context.Times.ToList())
        {
            var contestTime = new ContestTime()
            {
                ContestId = contest.Id,
                TimeId = time.Id,
            };
            context.ContestTimes.Add(contestTime);
        }
        
        await context.SaveChangesAsync();
    }


    static async void SetupClients(AppDbContext context, UserManager<AppUser> userManager)
    {
        var ALevel = await context.Levels.Where(e => e.Title == "A").FirstOrDefaultAsync();
        var BLevel = await context.Levels.Where(e => e.Title == "B").FirstOrDefaultAsync();
        var CLevel = await context.Levels.Where(e => e.Title == "C").FirstOrDefaultAsync();
        var DLevel = await context.Levels.Where(e => e.Title == "D").FirstOrDefaultAsync();
        var contest = await context.Contests.Where(e => e.Description == "Something").FirstOrDefaultAsync();

        foreach (var gameType in context.GameTypes.ToList())
        {
            var package = new PackageGameTypeTime();
            if (gameType.GameTypeName == "Paddle")
            {
                continue;
                package = await context.PackageGameTypeTimes.Where(e => e.PackageGtName == "Täielik paddle")
                    .FirstOrDefaultAsync();
            }
            else if (gameType.GameTypeName == "Tennis")
            {
                package = await context.PackageGameTypeTimes.Where(e => e.PackageGtName == "Täielik tennis")
                    .FirstOrDefaultAsync();
            }
            else
            {
                //rannatennise
                continue;
            }

            foreach (var level in context.Levels.Where(e =>
                             e.Id == ALevel!.Id)
                         .ToList())
            {
                for (int i = 0; i < 7; i++)
                {
                    var user = new AppUser()
                    {
                        Email = "paddle" + i + "@eesti.ee", UserName = "uus" + i + "@eesti.ee",
                        FirstName = i.ToString(),
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
                        context.Contests.Where(t => t.Id == contest.Id)
                            .FirstOrDefaultAsync().Result!.From.Date;
                    var contestEndDate =
                        context.Contests.Where(t => t.Id == contest.Id)
                            .FirstOrDefaultAsync().Result!.Until.Date;
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
}
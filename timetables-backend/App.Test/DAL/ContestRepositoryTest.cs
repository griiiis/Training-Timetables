using App.DAL.EF;
using App.DAL.EF.Repositories;
using App.Domain;
using App.Domain.Enums;
using App.Domain.Identity;
using AutoMapper;
using Base.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace App.Test.DAL;

[Collection("NonParallel")]
public class ContestRepositoryTest
{
    private readonly  DbContextOptions<AppDbContext> _contextOptions;
    private readonly  IMapper _mapper;
    private AppDbContext CreateContext() => new(_contextOptions);
    private readonly Guid _mainTestUserId;
    private readonly Guid _anotherTestUserId;
    private readonly ContestType contestType;
    private readonly GameType gameType;
    private readonly Level level;
    private readonly TimeOfDay timeOfDay;
    private readonly Time time;
    private readonly PackageGameTypeTime package;
    private readonly Location location;
    
    public ContestRepositoryTest()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        _contextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        using var ctx = new AppDbContext(_contextOptions);
        //_ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Contest, App.DAL.DTO.Contest>().ReverseMap();
            cfg.CreateMap<ContestType, App.DAL.DTO.ContestType>().ReverseMap();
            cfg.CreateMap<Location, App.DAL.DTO.Location>().ReverseMap();
        });
        _mapper = config.CreateMapper();

        //seeding
        _mainTestUserId = ctx.AppUsers.Add(new AppUser
        {
            Id = Guid.NewGuid(),
            FirstName = "Main App",
            LastName = "User",
            Age = 50,
            Gender = EGender.Male

        }).Entity.Id;
        _anotherTestUserId = ctx.AppUsers.Add(new AppUser()
        {
            Id = Guid.NewGuid(),
            FirstName = "Another App",
            LastName = "User",
            Age = 40,
            Gender = EGender.Female
        }).Entity.Id;
        
        //Contest Type
        contestType = ctx.ContestTypes.Add(new ContestType()
            { ContestTypeName = "Treening", Description = "Trenn", AppUserId = _mainTestUserId }).Entity;
        
        //Game Type
        gameType = ctx.GameTypes.Add(new GameType() { GameTypeName = "Tennis", AppUserId = _mainTestUserId }).Entity;
        
        //Levels
        level =ctx.Levels.Add(new Level() { Title = "A", Description = "Osav", AppUserId = _mainTestUserId }).Entity;
        
        //Time of Day
        timeOfDay =ctx.TimeOfDays.Add(new TimeOfDay() { TimeOfDayName = "Hommikune", AppUserId = _mainTestUserId }).Entity;
        
        //Time
        time =ctx.Times.Add(new Time()
        {
            TimeOfDay = timeOfDay,
            From = new TimeOnly(10, 00),
            Until = new TimeOnly(12, 30),
            AppUserId = _mainTestUserId,
        }).Entity;
        
        //Package
        package =ctx.PackageGameTypeTimes.Add(new PackageGameTypeTime()
        {
            PackageGtName = "Täielik tennis",
            GameType = gameType,
            Times = 1,
            AppUserId = _mainTestUserId,
        }).Entity;
        
        //Location
        location =ctx.Locations.Add(new Location()
        {
            LocationName = "Laagri väljakud",
            State = "Harjumaa",
            Country = "Eesti",
            AppUserId = _mainTestUserId,
        }).Entity;
        
        ctx.SaveChangesAsync();
    }
    
    [Fact]
    public async Task FirstOrDefaultAsync_Contest_Exists()
    {
        using var ctx = CreateContext();
        var contestRepository = new ContestRepository(ctx, _mapper);

        //arrange
        var contest = ctx.Add(new Contest
        {
            ContestName = "Test ContestName",
            Description = "Something",
            AppUserId = _mainTestUserId,
            ContestTypeId = contestType.Id, 
            LocationId = location.Id,    
            From = DateTime.Now,
            Until = DateTime.Now.AddDays(1)
        }).Entity;
        await ctx.SaveChangesAsync();

        //act
        var data = await contestRepository.FirstOrDefaultAsync(contest.Id);
        
        //assert
        Assert.NotNull(data);
        Assert.Equal(contest.ContestName, data.ContestName);
    }

    [Fact]
    public async Task GetAllAsync_WithoutUserId()
    {
        using var ctx = CreateContext();
        var contestRepository = new ContestRepository(ctx, _mapper);

        //arrange
        ctx.Add(new Contest
        {
            ContestName = "Test Contest 1",
            Description = "Something",
            AppUserId = _mainTestUserId,
            ContestTypeId = contestType.Id, 
            LocationId = location.Id,
            From = DateTime.Now,
            Until = DateTime.Now.AddDays(1)
        });
        ctx.Add(new Contest
        {
            ContestName = "Test Contest 2",
            Description = "Something",
            AppUserId = _anotherTestUserId,
            ContestTypeId = contestType.Id, 
            LocationId = location.Id,
            From = DateTime.Now,
            Until = DateTime.Now.AddDays(1)
        });
        await ctx.SaveChangesAsync();

        //act
        var data = await contestRepository.GetAllAsync();

        //assert
        Assert.Equal(2, data.Count());
    }

    [Fact]
    public async Task GetUserContests_WithUserId()
    {
        using var ctx = CreateContext();
        var contestRepository = new ContestRepository(ctx, _mapper);

        //arrange
        var contest = ctx.Add(new Contest
        {
            ContestName = "Test Contest 1",
            Description = "Something",
            AppUserId = _mainTestUserId,
            ContestTypeId = contestType.Id, 
            LocationId = location.Id, 
            From = DateTime.Now,
            Until = DateTime.Now.AddDays(1)
        }).Entity;
        ctx.Add(new Contest
        {
            ContestName = "Test Contest 2",
            AppUserId = _anotherTestUserId,
            Description = "Something",
            ContestTypeId = contestType.Id, 
            LocationId = location.Id, 
            From = DateTime.Now,
            Until = DateTime.Now.AddDays(1)
        });

        var team = ctx.Teams.Add(new Team
        {
            TeamName = "Test",
            LevelId = level.Id,
            GameTypeId = gameType.Id,
        }).Entity;

        ctx.UserContestPackages.Add(new UserContestPackage
        {
            PackageGameTypeTimeId = package.Id,
            HoursAvailable = 10,
            AppUserId = _mainTestUserId,
            ContestId = contest.Id,
            TeamId = team.Id,
            LevelId = level.Id,
        });

        await ctx.SaveChangesAsync();

        //act
        var data = await contestRepository.GetUserContests(_mainTestUserId);

        //assert
        Assert.Single(data); // Only contests related to _mainTestUserId should be returned
    }
    
    
}
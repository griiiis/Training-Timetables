using App.BLL.DTO;
using App.BLL.Services;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using NSubstitute;

namespace App.Test.BLL;
/*
[Collection("NonParallel")]

public class ContestServiceTest
{
    private readonly  IMapper _mapper;
    private readonly Guid _mainTestUserId = Guid.NewGuid();

    public ContestServiceTest()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Contest, App.DAL.DTO.Contest>().ReverseMap();
            cfg.CreateMap<ContestType, App.DAL.DTO.ContestType>().ReverseMap();
            cfg.CreateMap<Location, App.DAL.DTO.Location>().ReverseMap();
        });
        _mapper = config.CreateMapper();
        
    }
    
    [Fact]
    public async Task FirstOrDefaultAsync_Contest_Exists()
    {
        //arrange
        var testRepoMock = Substitute.For<IContestRepository>();
        
        var testEntityService =
            new ContestService(testRepoMock, _mapper);

        var id = Guid.NewGuid();
        
        await testEntityService.FirstOrDefaultAsync(id, _mainTestUserId);
        
        //assert
        await testRepoMock.Received().FirstOrDefaultAsync(id, _mainTestUserId);
    }
    
    [Fact]
    public async Task GetAllAsync_WithoutUserId()
    {
        //arrange
        var testRepoMock = Substitute.For<IContestRepository>();
        
        var testEntityService =
            new ContestService(testRepoMock, _mapper);
        
        await testEntityService.GetAllAsync(_mainTestUserId);
        
        //assert
        await testRepoMock.Received().GetAllAsync(_mainTestUserId);
    }
    
    [Fact]
    public async Task GetUserContests_WithUserId()
    {
        //arrange
        var testRepoMock = Substitute.For<IContestRepository>();
        
        var testEntityService =
            new ContestService(testRepoMock, _mapper);
        
        await testEntityService.GetUserContests(_mainTestUserId);
        
        //assert
        await testRepoMock.Received().GetUserContests(_mainTestUserId);
    }
    
    [Fact]
    public void AddContestWithUser()
    {
        //arrange
        var testRepoMock = Substitute.For<IContestRepository>();
        
        var testEntityService =
            new ContestService(testRepoMock, _mapper);
        
        var contest = new Contest
        {
            Id = Guid.NewGuid(),
            ContestName = "Test Contest 1",
            Description = "Something",
            ContestTypeId = Guid.NewGuid(), 
            LocationId = Guid.NewGuid(), 
            From = DateTime.Now,
            Until = DateTime.Now.AddDays(1)
        };
        
        //Act
        testEntityService.AddContestWithUser(_mainTestUserId, contest);
        
        //assert
        testRepoMock.Received().Add(Arg.Any<App.DAL.DTO.Contest>());
    }
    
    [Fact]
    public void IsContestOwnedByUser()
    {
        // Arrange
        var testRepoMock = Substitute.For<IContestRepository>();
        var testEntityService = new ContestService(testRepoMock, _mapper);
        var id = Guid.NewGuid();
        var contest = new App.DAL.DTO.Contest
        {
            Id = id, 
            AppUserId = _mainTestUserId
        };
        testRepoMock.FirstOrDefault(id).Returns(contest);

        // Act
        var result = testEntityService.IsContestOwnedByUser(_mainTestUserId, id);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void UpdateContestWithUser()
    {
        //arrange
        var testRepoMock = Substitute.For<IContestRepository>();
        
        var testEntityService =
            new ContestService(testRepoMock, _mapper);
        
        var contest = new Contest
        {
            ContestName = "Test Contest 1",
            Description = "Something",
            ContestTypeId = Guid.NewGuid(), 
            LocationId = Guid.NewGuid(), 
            From = DateTime.Now,
            Until = DateTime.Now.AddDays(1)
        };
        // Act
        testEntityService.UpdateContestWithUser(_mainTestUserId, contest);
        
        //assert
        testRepoMock.Received().Update(Arg.Any<App.DAL.DTO.Contest>());
    }
    
}*/
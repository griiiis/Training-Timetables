using AutoMapper;
using Base.Test.DAL;
using NSubstitute;

namespace Base.Test.BLL;
/*
public class BaseServiceTest
{
    
    private readonly  IMapper _mapper;
    private readonly Guid _mainTestUserId = Guid.NewGuid();

    public BaseServiceTest()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DAL.TestEntity, TestEntity>().ReverseMap();
            cfg.CreateMap<DAL.TestUser, TestUser>().ReverseMap();
            cfg.CreateMap<DAL.TestEntity, Domain.TestEntity>().ReverseMap();
            cfg.CreateMap<DAL.TestUser, Domain.TestUser>().ReverseMap();
        });
        _mapper = config.CreateMapper();
    }
    
    [Fact]
    public void Add_Entity()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);
        
        var testEntity = new DAL.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        };

        testEntityService.Add(_mapper.Map<DAL.TestEntity, TestEntity>(testEntity));
        
        //assert
        testRepoMock.Received().Add(testEntity);
    }
    
    [Fact]
    public void Update_Entity()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);
        
        var testEntity = new DAL.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        };
        
        testEntityService.Update(_mapper.Map<DAL.TestEntity, TestEntity>(testEntity));
        
        //assert
        testRepoMock.Received().Update(testEntity);
    }
    
    [Fact]
    public void Remove_Entity()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);
        
        var testEntity = new DAL.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        };

        testEntityService.Remove(_mapper.Map<DAL.TestEntity, TestEntity>(testEntity));
        
        //assert
        testRepoMock.Received().Remove(testEntity);
    }
    
    [Fact]
    public void Remove_Entity_UserId()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);
        
        var testEntity = new DAL.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        };

        testEntityService.Remove(_mapper.Map<DAL.TestEntity, TestEntity>(testEntity), _mainTestUserId);
        
        //assert
        testRepoMock.Received().Remove(testEntity, _mainTestUserId);
    }
    
    [Fact]
    public async Task Remove_EntityAsync_EntityId()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);
        
        var id = Guid.NewGuid();
        
        await testEntityService.RemoveAsync(id);
        
        //assert
        await testRepoMock.Received().RemoveAsync(id);
    }
    
    [Fact]
    public async Task Remove_EntityAsync()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);
        
        var testEntity = new DAL.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        };

        await testEntityService.RemoveAsync(_mapper.Map<DAL.TestEntity, TestEntity>(testEntity));
        
        //assert
        await testRepoMock.Received().RemoveAsync(testEntity);
    }
    
    [Fact]
    public async Task Remove_EntityAsync_UserId()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);
        
        var testEntity = new DAL.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        };

        await testEntityService.RemoveAsync(_mapper.Map<DAL.TestEntity, TestEntity>(testEntity), _mainTestUserId);
        
        //assert
        await testRepoMock.Received().RemoveAsync(testEntity, _mainTestUserId);
    }
    
    [Fact]
    public async Task Remove_EntityAsync_EntityId_UserId()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);

        var id = Guid.NewGuid();

        await testEntityService.RemoveAsync(id, _mainTestUserId);
        
        //assert
        await testRepoMock.Received().RemoveAsync(id, _mainTestUserId);
    }
    
    [Fact]
    public void Remove_Entity_EntityId()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);
        
        var id = Guid.NewGuid();

        testEntityService.Remove(id);
        
        //assert
        testRepoMock.Received().Remove(id);
    }
    
    [Fact]
    public void Remove_Entity_EntityId_UserId()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);

        var id = Guid.NewGuid();

        testEntityService.Remove(id, _mainTestUserId);
        
        //assert
        testRepoMock.Received().Remove(id, _mainTestUserId);
    }
    
    [Fact]
    public void Get_All()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);
        

        testEntityService.GetAll(_mainTestUserId);
        
        //assert
        testRepoMock.Received().GetAll(_mainTestUserId);
    }
    
    [Fact]
    public void GetAll_WithoutUserId()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);

        testEntityService.GetAll();
        
        //assert
        testRepoMock.Received().GetAll();
    }
    
    [Fact]
    public void Exists_Entity()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);

        var id = Guid.NewGuid();

        testEntityService.Exists(id);
        
        //assert
        testRepoMock.Received().Exists(id);
    }
    
    [Fact]
    public void Exists_Entity_WithUserId()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);
        
        var id = Guid.NewGuid();

        testEntityService.Exists(id, _mainTestUserId);
        
        //assert
        testRepoMock.Received().Exists(id,_mainTestUserId);
    }
    
    [Fact]
    public async Task ExistsAsync_Entity_WithUserId()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);
        
        var id = Guid.NewGuid();

        await testEntityService.ExistsAsync(id, _mainTestUserId);
        
        //assert
        await testRepoMock.Received().ExistsAsync(id,_mainTestUserId);
    }
    
    [Fact]
    public void FirstOrDefault_Entity_Exists()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);

        var id = Guid.NewGuid();

        testEntityService.FirstOrDefault(id);
        
        //assert
        testRepoMock.Received().FirstOrDefault(id);
    }
    
    [Fact]
    public async Task FirstOrDefaultAsync_Entity_Exists()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);

        var id = Guid.NewGuid();

        await testEntityService.FirstOrDefaultAsync(id);
        
        //assert
        await testRepoMock.Received().FirstOrDefaultAsync(id);
    }
    
    [Fact]
    public async Task GetAllAsync_WithoutUserId()
    {
        //arrange
        var testRepoMock = Substitute.For<ITestEntityRepository>();
        
        var testEntityService =
            new TestEntityService(testRepoMock, _mapper);

        await testEntityService.GetAllAsync();
        
        //assert
        await testRepoMock.Received().GetAllAsync();
    }
}*/
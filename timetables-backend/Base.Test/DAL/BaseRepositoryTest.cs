using AutoMapper;
using Base.DAL.EF;
using Base.Test.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Base.Test.DAL;

public class BaseRepositoryTest
{
    private readonly DbContextOptions<TestDbContext> _contextOptions;
    private readonly BaseDalDomainMapper<DAL.TestEntity, Domain.TestEntity> _DomainDalmapper;
    private readonly IMapper _mapper;
    private TestDbContext CreateContext() => new(_contextOptions);
    private readonly Guid _mainTestUserId;
    private readonly Guid _anotherTestUserId;

    public BaseRepositoryTest()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        _contextOptions = new DbContextOptionsBuilder<TestDbContext>().UseSqlite(connection).Options;
        using var ctx = new TestDbContext(_contextOptions);
        //_ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();

        var config = new MapperConfiguration(cfg => cfg.CreateMap<DAL.TestEntity, Domain.TestEntity>().ReverseMap());
        _mapper = config.CreateMapper();
        _DomainDalmapper = new BaseDalDomainMapper<DAL.TestEntity, Domain.TestEntity>(_mapper);

        //seeding
        _mainTestUserId = ctx.TestUsers.Add(new Domain.TestUser()
        {
            Id = Guid.NewGuid(),
            Name = "Main Test user"
        }).Entity.Id;
        _anotherTestUserId = ctx.TestUsers.Add(new Domain.TestUser()
        {
            Id = Guid.NewGuid(),
            Name = "Another Test user"
        }).Entity.Id;
        ctx.SaveChangesAsync();
    }

    [Fact]
    public async Task Add_Entity()
    {
        await using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = testEntityRepository.Add(new TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        });
        await ctx.SaveChangesAsync();

        //act
        var data = await ctx.TestEntities.FindAsync(testEntity.Id);

        //assert
        Assert.NotNull(data);
    }

    [Fact]
    public async Task Update_Entity()
    {
        await using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        }).Entity;
        await ctx.SaveChangesAsync();

        var data = await ctx.TestEntities.FindAsync(testEntity.Id);
        data!.Value = "New Value";

        ctx.Entry(testEntity).State = EntityState.Detached;

        testEntity =
            _mapper.Map<DAL.TestEntity, Domain.TestEntity>(
                testEntityRepository.Update(_mapper.Map<Domain.TestEntity, DAL.TestEntity>(data)));
        await ctx.SaveChangesAsync();

        //act
        data = await ctx.TestEntities.FindAsync(testEntity.Id);


        //assert
        Assert.NotNull(data);
        Assert.Equal("New Value", data.Value);
    }

    [Fact]
    public async Task Remove_Entity()
    {
        using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        }).Entity;
        await ctx.SaveChangesAsync();

        var data = await ctx.TestEntities.FindAsync(testEntity.Id);
        ctx.Entry(testEntity).State = EntityState.Detached;

        var num = testEntityRepository.Remove(_mapper.Map<Domain.TestEntity, DAL.TestEntity>(data));
        await ctx.SaveChangesAsync();

        //act
        data = await ctx.TestEntities.FindAsync(testEntity.Id);

        //assert
        Assert.Null(data);
        Assert.Equal(1, num);
    }


    [Fact]
    public async Task Remove_Entity_UserId()
    {
        using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        }).Entity;
        await ctx.SaveChangesAsync();

        var data = await ctx.TestEntities.FindAsync(testEntity.Id);
        ctx.Entry(testEntity).State = EntityState.Detached;

        testEntityRepository.Remove(_mapper.Map<Domain.TestEntity, DAL.TestEntity>(data), _mainTestUserId);
        await ctx.SaveChangesAsync();

        //act
        data = await ctx.TestEntities.FindAsync(testEntity.Id);

        //assert
        Assert.Null(data);
    }

    [Fact]
    public async Task Remove_EntityAsync_EntityId()
    {
        using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        }).Entity;
        await ctx.SaveChangesAsync();

        var data = await ctx.TestEntities.FindAsync(testEntity.Id);
        ctx.Entry(testEntity).State = EntityState.Detached;

        var num = await testEntityRepository.RemoveAsync(data!.Id);
        await ctx.SaveChangesAsync();

        //act
        data = await ctx.TestEntities.FindAsync(testEntity.Id);

        //assert
        Assert.Null(data);
        Assert.Equal(1, num);
    }


    [Fact]
    public async Task Remove_EntityAsync()
    {
        await using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        }).Entity;
        await ctx.SaveChangesAsync();

        var data = await ctx.TestEntities.FindAsync(testEntity.Id);
        ctx.Entry(testEntity).State = EntityState.Detached;

        var num = await testEntityRepository.RemoveAsync(_mapper.Map<Domain.TestEntity, DAL.TestEntity>(data));
        await ctx.SaveChangesAsync();

        //act
        data = await ctx.TestEntities.FindAsync(testEntity.Id);

        //assert
        Assert.Null(data);
        Assert.Equal(1, num);
    }

    [Fact]
    public async Task Remove_EntityAsync_UserId()
    {
        await using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        }).Entity;
        await ctx.SaveChangesAsync();

        var data = await ctx.TestEntities.FindAsync(testEntity.Id);
        ctx.Entry(testEntity).State = EntityState.Detached;

        var num = await testEntityRepository.RemoveAsync(_mapper.Map<Domain.TestEntity, DAL.TestEntity>(data),
            _mainTestUserId);
        await ctx.SaveChangesAsync();

        //act
        data = await ctx.TestEntities.FindAsync(testEntity.Id);

        //assert
        Assert.Null(data);
        Assert.Equal(1, num);
    }

    [Fact]
    public async Task Remove_EntityAsync_EntityId_UserId()
    {
        using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        }).Entity;
        await ctx.SaveChangesAsync();

        var data = await ctx.TestEntities.FindAsync(testEntity.Id);
        ctx.Entry(testEntity).State = EntityState.Detached;

        var num = await testEntityRepository.RemoveAsync(data!.Id, _mainTestUserId);
        await ctx.SaveChangesAsync();

        //act
        data = await ctx.TestEntities.FindAsync(testEntity.Id);

        //assert
        Assert.Null(data);
        Assert.Equal(1, num);
    }


    [Fact]
    public async Task Remove_Entity_EntityId()
    {
        await using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        }).Entity;
        await ctx.SaveChangesAsync();

        var data = await ctx.TestEntities.FindAsync(testEntity.Id);
        ctx.Entry(testEntity).State = EntityState.Detached;

        var num = testEntityRepository.Remove(data!.Id);
        await ctx.SaveChangesAsync();

        //act
        data = await ctx.TestEntities.FindAsync(testEntity.Id);

        //assert
        Assert.Null(data);
        Assert.Equal(1, num);
    }

    [Fact]
    public async Task Remove_Entity_EntityId_UserId()
    {
        await using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        }).Entity;
        await ctx.SaveChangesAsync();

        var data = await ctx.TestEntities.FindAsync(testEntity.Id);
        ctx.Entry(testEntity).State = EntityState.Detached;

        var num = testEntityRepository.Remove(data!.Id, _mainTestUserId);
        await ctx.SaveChangesAsync();

        //act
        data = await ctx.TestEntities.FindAsync(testEntity.Id);

        //assert
        Assert.Null(data);
        Assert.Equal(1, num);
    }

    [Fact]
    public async Task GetAll_WithUserId()
    {
        await using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        });
        ctx.Add(new Domain.TestEntity
        {
            Value = "Bar",
            AppUserId = _mainTestUserId,
        });
        ctx.Add(new Domain.TestEntity
        {
            Value = "Baz",
            AppUserId = _anotherTestUserId,
        });
        await ctx.SaveChangesAsync();

        //act
        var data = testEntityRepository.GetAll(_mainTestUserId);

        //assert
        Assert.Equal(2, data.Count());
    }

    [Fact]
    public async Task GetAll_WithoutUserId()
    {
        await using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        });
        ctx.Add(new Domain.TestEntity
        {
            Value = "Bar",
            AppUserId = _mainTestUserId,
        });
        ctx.Add(new Domain.TestEntity
        {
            Value = "Baz",
            AppUserId = _anotherTestUserId,
        });
        await ctx.SaveChangesAsync();

        //act
        var data = testEntityRepository.GetAll();

        //assert
        Assert.Equal(3, data.Count());
    }

    [Fact]
    public async Task Exists_Entity()
    {
        await using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        }).Entity;
        await ctx.SaveChangesAsync();

        //act
        var exists = testEntityRepository.Exists(testEntity.Id);

        //assert
        Assert.True(exists);
    }

    [Fact]
    public async Task DoesNotExist_Entity()
    {
        await using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //act
        var exists = testEntityRepository.Exists(Guid.NewGuid());

        //assert
        Assert.False(exists);
    }

    [Fact]
    public async Task Exists_Entity_WithUserId()
    {
        await using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        }).Entity;
        await ctx.SaveChangesAsync();

        //act
        var exists = testEntityRepository.Exists(testEntity.Id, _mainTestUserId);

        //assert
        Assert.True(exists);
    }

    [Fact]
    public async Task DoesNotExist_Entity_WithUserId()
    {
        await using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //act
        var exists = testEntityRepository.Exists(Guid.NewGuid(), _mainTestUserId);

        //assert
        Assert.False(exists);
    }

    [Fact]
    public async Task FirstOrDefault_Entity_Exists()
    {
        using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        }).Entity;
        await ctx.SaveChangesAsync();

        //act
        var data = testEntityRepository.FirstOrDefault(testEntity.Id);

        //assert
        Assert.NotNull(data);
    }

    [Fact]
    public async Task FirstOrDefault_Entity_DoesNotExist()
    {
        using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //act
        var data = testEntityRepository.FirstOrDefault(Guid.NewGuid());

        //assert
        Assert.Null(data);
    }

    [Fact]
    public async Task FirstOrDefaultAsync_Entity_Exists()
    {
        using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        }).Entity;
        await ctx.SaveChangesAsync();

        //act
        var data = await testEntityRepository.FirstOrDefaultAsync(testEntity.Id);

        //assert
        Assert.NotNull(data);
    }

    [Fact]
    public async Task FirstOrDefaultAsync_Entity_DoesNotExist()
    {
        using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //act
        var data = await testEntityRepository.FirstOrDefaultAsync(Guid.NewGuid());

        //assert
        Assert.Null(data);
    }

    [Fact]
    public async Task GetAllAsync_WithoutUserId()
    {
        using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        });
        ctx.Add(new Domain.TestEntity
        {
            Value = "Bar",
            AppUserId = _mainTestUserId,
        });
        ctx.Add(new Domain.TestEntity
        {
            Value = "Baz",
            AppUserId = _anotherTestUserId,
        });
        await ctx.SaveChangesAsync();

        //act
        var data = await testEntityRepository.GetAllAsync();

        //assert
        Assert.Equal(3, data.Count());
    }

    [Fact]
    public async Task GetAllAsync_WithUserId()
    {
        using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        });
        ctx.Add(new Domain.TestEntity
        {
            Value = "Bar",
            AppUserId = _mainTestUserId,
        });
        ctx.Add(new Domain.TestEntity
        {
            Value = "Baz",
            AppUserId = _anotherTestUserId,
        });
        await ctx.SaveChangesAsync();

        //act
        var data = await testEntityRepository.GetAllAsync(_mainTestUserId);

        //assert
        Assert.Equal(2, data.Count());
    }

    [Fact]
    public async Task ExistsAsync_Entity_Exists()
    {
        using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        }).Entity;
        await ctx.SaveChangesAsync();

        //act
        var exists = await testEntityRepository.ExistsAsync(testEntity.Id);

        //assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsAsync_Entity_DoesNotExist()
    {
        using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //act
        var exists = await testEntityRepository.ExistsAsync(Guid.NewGuid());

        //assert
        Assert.False(exists);
    }

    [Fact]
    public async Task ExistsAsync_Entity_WithUserId_Exists()
    {
        using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //arrange
        var testEntity = ctx.Add(new Domain.TestEntity
        {
            Value = "Foo",
            AppUserId = _mainTestUserId,
        }).Entity;
        await ctx.SaveChangesAsync();

        //act
        var exists = await testEntityRepository.ExistsAsync(testEntity.Id, _mainTestUserId);

        //assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsAsync_Entity_WithUserId_DoesNotExist()
    {
        using var ctx = CreateContext();
        var testEntityRepository = new TestEntityRepository(ctx, _mapper);

        //act
        var exists = await testEntityRepository.ExistsAsync(Guid.NewGuid(), _mainTestUserId);

        //assert
        Assert.False(exists);
    }
}
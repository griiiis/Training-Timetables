using AutoMapper;
using Base.DAL.EF;
using Base.Test.Domain;

namespace Base.Test.DAL;

public class TestEntityRepository : BaseEntityRepository<Domain.TestEntity, TestEntity, TestDbContext>
{
    public TestEntityRepository(TestDbContext dbContext, IMapper mapper) : base(dbContext, new BaseDalDomainMapper<Domain.TestEntity, TestEntity>(mapper))
    {
    }
}
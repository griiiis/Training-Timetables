using Base.Contracts.BLL;
using Base.Contracts.DAL;
using Base.Contracts.Domain;

namespace Base.BLL;

public class BaseEntityService<TDalEntity, TBLLEntity, TRepository, TUoW>
    : BaseEntityService<TDalEntity, TBLLEntity, TRepository, Guid, TUoW>, IEntityService<TBLLEntity>
    where TBLLEntity : class, IDomainEntityId
    where TRepository : IEntityRepository<TDalEntity, Guid>
    where TDalEntity : class, IDomainEntityId<Guid>
    where TUoW : IUnitOfWork
{
    public BaseEntityService(TUoW uow, TRepository repository, IBLLMapper<TDalEntity, TBLLEntity> Mapper) : base(uow, repository, Mapper)
    {
    }
}

public class BaseEntityService<TDalEntity, TBLLEntity, TRepository, TKey, TUoW>
    : IEntityService<TBLLEntity, TKey>
    where TKey : IEquatable<TKey>
    where TBLLEntity : class, IDomainEntityId<TKey>
where TRepository : IEntityRepository<TDalEntity, TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TUoW : IUnitOfWork
{
    protected readonly TUoW Uow;
    protected readonly TRepository Repository;
    protected readonly IBLLMapper<TDalEntity, TBLLEntity> Mapper;

    public BaseEntityService(TUoW uow, TRepository repository, IBLLMapper<TDalEntity, TBLLEntity> mapper)
    {
        Uow = uow;
        Repository = repository;
        Mapper = mapper;
    }
    
    
    public TBLLEntity Add(TBLLEntity entity)
    {
        return Mapper.Map(Repository.Add(Mapper.Map(entity)))!;
    }

    public TBLLEntity Update(TBLLEntity entity)
    {
        return Mapper.Map(Repository.Update(Mapper.Map(entity)))!;
    }

    public int Remove(TBLLEntity entity, TKey? userId = default)
    {
        return Repository.Remove(Mapper.Map(entity), userId);
    }

    public int Remove(TKey id, TKey? userId = default)
    {
        return Repository.Remove(id, userId);
    }

    public async Task<int> RemoveAsync(TBLLEntity entity, TKey? userId = default)
    {
        return await Repository.RemoveAsync(Mapper.Map(entity), userId);
    }

    public async Task<int> RemoveAsync(TKey id, TKey? userId = default)
    {
        return await Repository.RemoveAsync(id, userId);
    }

    public TBLLEntity? FirstOrDefault(TKey id, TKey? userId = default, bool noTracking = true)
    {
        return Mapper.Map(Repository.FirstOrDefault(id, userId, noTracking));
    }

    public IEnumerable<TBLLEntity> GetAll(TKey? userId = default, bool noTracking = true)
    {
        return Repository.GetAll(userId, noTracking).Select(e => Mapper.Map(e));
    }

    public bool Exists(TKey id, TKey? userId = default)
    {
        return Repository.Exists(id, userId);
    }

    public async Task<TBLLEntity?> FirstOrDefaultAsync(TKey id, TKey? userId = default, bool noTracking = true)
    {
        return Mapper.Map(await Repository.FirstOrDefaultAsync(id, userId, noTracking));
    }

    public async Task<IEnumerable<TBLLEntity>> GetAllAsync(TKey? userId = default, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(e => Mapper.Map(e));
    }

    public async Task<bool> ExistsAsync(TKey id, TKey? userId = default)
    {
        return await Repository.ExistsAsync(id, userId);
    }
}
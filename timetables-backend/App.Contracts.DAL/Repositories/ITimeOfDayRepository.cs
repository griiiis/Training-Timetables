using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ITimeOfDayRepository : IEntityRepository<DALDTO.TimeOfDay>, ITimeOfDayRepositoryCustom<DALDTO.TimeOfDay>
{
}

public interface ITimeOfDayRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetContestTimeOfDays(Guid contestId);
}
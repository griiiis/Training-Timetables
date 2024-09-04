using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IUserContestPackageRepository : IEntityRepository<DALDTO.UserContestPackage>, IUserContestPackageRepositoryCustom<DALDTO.UserContestPackage>
{
}

public interface IUserContestPackageRepositoryCustom<TEntity>
{
    Task<TEntity?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetUserContestPackage(Guid contestId, Guid userId);
    Task<IEnumerable<TEntity>> GetContestUsers(Guid contestId);
    Task<IEnumerable<TEntity>> GetCurrentUserPackages(Guid userId);
    Task<IEnumerable<TEntity>> GetContestTeachers(Guid contestId);
    Task<IEnumerable<TEntity>> GetContestUsersWithoutTeachers(Guid contestId);
    Task<IEnumerable<TEntity>> GetContestUsersWithoutTeachers(Guid contestId, Guid teamId);
    bool AnyTeams(Guid contestId);
    bool IfAlreadyJoined(Guid contestId, Guid userId);

}
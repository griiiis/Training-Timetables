using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class UserContestPackageService : BaseEntityService<App.DAL.DTO.UserContestPackage, App.BLL.DTO.UserContestPackage, IUserContestPackageRepository, IAppUnitOfWork>, IUserContestPackageService
{
    public UserContestPackageService(IAppUnitOfWork uow, IUserContestPackageRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.UserContestPackage, App.BLL.DTO.UserContestPackage>(mapper))
    {
        
    }

    public async Task<IEnumerable<UserContestPackage>> GetAllAsync()
    {
        return (await Repository.GetAllAsync()).Select(de => Mapper.Map(de))!;
    }

    public async Task<UserContestPackage?> GetUserContestPackage(Guid contestId, Guid userId)
    {
        return Mapper.Map(await Repository.GetUserContestPackage(contestId, userId));
    }
    
    public async Task<IEnumerable<UserContestPackage>> GetContestUsers(Guid contestId)
    {
        return (await Repository.GetContestUsers(contestId)).Select(de => Mapper.Map(de))!;
    }

    public async Task<IEnumerable<UserContestPackage>> GetCurrentUserPackages(Guid userId)
    {
        return (await Repository.GetCurrentUserPackages(userId)).Select(de => Mapper.Map(de))!;
    }

    public async  Task<IEnumerable<UserContestPackage>> GetContestTeachers(Guid contestId)
    {
        return (await Repository.GetContestTeachers(contestId)).Select(de => Mapper.Map(de))!;
    }

    public async Task<IEnumerable<UserContestPackage>> GetContestTeammates(Guid contestId, Guid teamId)
    {
        return (await Repository.GetContestTeammates(contestId, teamId)).Select(de => Mapper.Map(de)!);
    }
    public async Task<IEnumerable<UserContestPackage>> GetContestParticipants(Guid contestId)
    {
        return (await Repository.GetContestParticipants(contestId)).Select(de => Mapper.Map(de)!);
    }
    
    public bool AnyTeams(Guid contestId)
    {
        return Repository.AnyTeams(contestId);
    }
    
    public bool IfAlreadyJoined(Guid contestId, Guid userId)
    {
        return Repository.IfAlreadyJoined(contestId, userId);
    }

    public UserContestPackage AddPackageWithUser(Guid userId, UserContestPackage userContestPackage)
    {
        var dto = Mapper.Map(userContestPackage)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Add(dto))!;
    }

    public bool IsPackageOwnedByUser(Guid userId, Guid userContestPackageId)
    {
        return Repository.FirstOrDefault(userContestPackageId)?.AppUserId == userId;
    }
    
    public UserContestPackage UpdatePackageWithUser(Guid userId, UserContestPackage userContestPackage)
    {
        var dto = Mapper.Map(userContestPackage)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Update(dto))!;
    }
}
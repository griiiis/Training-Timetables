using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class ContestService : BaseEntityService<App.DAL.DTO.Contest, App.BLL.DTO.Contest, IContestRepository, IAppUnitOfWork>, IContestService
{
    public ContestService(IAppUnitOfWork uow, IContestRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.Contest, App.BLL.DTO.Contest>(mapper))
    {
    }
    
    public async Task<App.BLL.DTO.Contest?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
    {
        return Mapper.Map(await Repository.FirstOrDefaultAsync(id, userId));
    }

    public new async Task<IEnumerable<App.BLL.DTO.Contest>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId)).Select(de => Mapper.Map(de));
    }

    public async Task<IEnumerable<Contest>> GetUserContests(Guid userId)
    {
        return (await Repository.GetUserContests(userId)).Select(de => Mapper.Map(de));
    }
    
    public App.BLL.DTO.Contest AddContestWithUser(Guid userId, App.BLL.DTO.Contest contest)
    {
        var dto = Mapper.Map(contest)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Add(dto))!;
    }

    public bool IsContestOwnedByUser(Guid userId, Guid contestId)
    {
        return Repository.FirstOrDefault(contestId)?.AppUserId == userId;
    }

    public Contest UpdateContestWithUser(Guid userId, Contest contest)
    {
        var dto = Mapper.Map(contest)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Update(dto))!;
    }
}
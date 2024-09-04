using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class LevelService : BaseEntityService<App.DAL.DTO.Level, App.BLL.DTO.Level, ILevelRepository, IAppUnitOfWork>, ILevelService
{
    public LevelService(IAppUnitOfWork uow, ILevelRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.Level, App.BLL.DTO.Level>(mapper))
    {
    }
    
    public new async Task<IEnumerable<Level>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId)).Select(de => Mapper.Map(de)!);
    }

    public async Task<IEnumerable<Level>> GetAllCurrentContestAsync(Guid contestId = default, bool noTracking = true)
    {
        return (await Repository.GetAllCurrentContestAsync(contestId)).Select(de => Mapper.Map(de)!);
    }

    public Level AddLevelWithUser(Guid userId, Level level)
    {
        var dto = Mapper.Map(level)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Add(dto))!;
    }

    public bool IsLevelOwnedByUser(Guid userId, Guid levelId)
    {
        return Repository.FirstOrDefault(levelId)?.AppUserId == userId;
    }

    public Level UpdateLevelWithUser(Guid userId, Level level)
    {
        var dto = Mapper.Map(level)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Update(dto))!;
    }
}
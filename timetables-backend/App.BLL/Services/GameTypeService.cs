using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class GameTypeService : BaseEntityService<App.DAL.DTO.GameType, App.BLL.DTO.GameType, IGameTypeRepository, IAppUnitOfWork>, IGameTypeService
{
    public GameTypeService(IAppUnitOfWork uow, IGameTypeRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.GameType, App.BLL.DTO.GameType>(mapper))
    {
    }

    public async Task<IEnumerable<BLL.DTO.GameType>> GetAllCurrentContestAsync(Guid contestId)
    {
        return (await Repository.GetAllCurrentContestAsync(contestId)).Select(de => Mapper.Map(de));
    }
    
    public App.BLL.DTO.GameType AddGameTypeWithUser(Guid userId, GameType gameType)
    {
        var dto = Mapper.Map(gameType)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Add(dto))!;
    }

    public bool IsGameTypeOwnedByUser(Guid userId, Guid gameTypeId)
    {
        return Repository.FirstOrDefault(gameTypeId)?.AppUserId == userId;
    }

    public GameType UpdateGameTypeWithUser(Guid userId, GameType gameType)
    {
        var dto = Mapper.Map(gameType)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Update(dto))!;
    }
}
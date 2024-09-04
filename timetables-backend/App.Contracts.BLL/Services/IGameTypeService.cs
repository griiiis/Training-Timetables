using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IGameTypeService : IEntityService<App.BLL.DTO.GameType>, IGameTypeRepositoryCustom<App.BLL.DTO.GameType>
{ 
    App.BLL.DTO.GameType AddGameTypeWithUser(Guid userId, App.BLL.DTO.GameType gameType);
    bool IsGameTypeOwnedByUser (Guid userId, Guid gameTypeId);
    App.BLL.DTO.GameType UpdateGameTypeWithUser(Guid userId, App.BLL.DTO.GameType gameType);
}
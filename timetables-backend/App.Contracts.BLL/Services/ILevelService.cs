using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ILevelService : IEntityService<App.BLL.DTO.Level>, ILevelRepositoryCustom<App.BLL.DTO.Level>
{ 
    App.BLL.DTO.Level AddLevelWithUser(Guid userId, App.BLL.DTO.Level level);
    bool IsLevelOwnedByUser (Guid userId, Guid levelId);
    App.BLL.DTO.Level UpdateLevelWithUser(Guid userId, App.BLL.DTO.Level level);
}
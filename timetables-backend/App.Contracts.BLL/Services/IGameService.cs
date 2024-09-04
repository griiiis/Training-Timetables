using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IGameService : IEntityService<App.BLL.DTO.Game>, IGameRepositoryCustom<App.BLL.DTO.Game>
{
    void CreateGames(App.BLL.DTO.Models.CreateGamesData gamesData, Guid contestId);
}
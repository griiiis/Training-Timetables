using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ICourtService : IEntityService<App.BLL.DTO.Court>, ICourtRepositoryCustom<App.BLL.DTO.Court>
{ 
    App.BLL.DTO.Court AddCourtWithUser(Guid userId, App.BLL.DTO.Court court);
    bool IsCourtOwnedByUser (Guid userId, Guid courtId);
    App.BLL.DTO.Court UpdateCourtWithUser(Guid userId, App.BLL.DTO.Court court);
}
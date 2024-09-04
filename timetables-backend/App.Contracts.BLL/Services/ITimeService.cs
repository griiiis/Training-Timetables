using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ITimeService : IEntityService<App.BLL.DTO.Time>, ITimeRepositoryCustom<App.BLL.DTO.Time>
{ 
    App.BLL.DTO.Time AddTimeWithUser(Guid userId, App.BLL.DTO.Time time);
    bool IsTimeOwnedByUser (Guid userId, Guid timeId);
    App.BLL.DTO.Time UpdateTimeWithUser(Guid userId, App.BLL.DTO.Time time);
}
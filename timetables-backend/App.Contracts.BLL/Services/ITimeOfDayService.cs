using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ITimeOfDayService : IEntityService<App.BLL.DTO.TimeOfDay>, ITimeOfDayRepositoryCustom<App.BLL.DTO.TimeOfDay>
{ 
    App.BLL.DTO.TimeOfDay AddTimeOfDayWithUser(Guid userId, App.BLL.DTO.TimeOfDay timeOfDay);
    bool IsTimeOfDayOwnedByUser (Guid userId, Guid timeOfDayId);
    App.BLL.DTO.TimeOfDay UpdateTimeOfDayWithUser(Guid userId, App.BLL.DTO.TimeOfDay timeOfDay);
}
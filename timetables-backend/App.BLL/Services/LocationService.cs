using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class LocationService : BaseEntityService<App.DAL.DTO.Location, App.BLL.DTO.Location, ILocationRepository, IAppUnitOfWork>, ILocationService
{
    public LocationService(IAppUnitOfWork uow, ILocationRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.Location, App.BLL.DTO.Location>(mapper))
    {
    }

    public Location AddLocationWithUser(Guid userId, Location location)
    {
        var dto = Mapper.Map(location)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Add(dto))!;
    }

    public bool IsLocationOwnedByUser(Guid userId, Guid locationId)
    {
        return Repository.FirstOrDefault(locationId)?.AppUserId == userId;
    }

    public Location UpdateLocationWithUser(Guid userId, Location location)
    {
        var dto = Mapper.Map(location)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Update(dto))!;
    }
}
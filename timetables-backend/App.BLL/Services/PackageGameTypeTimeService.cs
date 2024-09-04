using App.BLL;
using App.BLL.DTO;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public class PackageGameTypeTimeService : BaseEntityService<App.DAL.DTO.PackageGameTypeTime, App.BLL.DTO.PackageGameTypeTime, IPackageGameTypeTimeRepository, IAppUnitOfWork>, IPackageGameTypeTimeService
{
    public PackageGameTypeTimeService(IAppUnitOfWork uow, IPackageGameTypeTimeRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.PackageGameTypeTime, App.BLL.DTO.PackageGameTypeTime>(mapper))
    {
    }

    public async Task<IEnumerable<PackageGameTypeTime>> GetAllCurrentContestAsync(Guid contestId)
    {
        return (await Repository.GetAllCurrentContestAsync(contestId)).Select(de => Mapper.Map(de));
    }

    public PackageGameTypeTime AddPackageGameTypeTimeWithUser(Guid userId, PackageGameTypeTime packageGameTypeTime)
    {
        var dto = Mapper.Map(packageGameTypeTime)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Add(dto))!;
    }

    public bool IsPackageGameTypeTimeOwnedByUser(Guid userId, Guid packageGameTypeTimeId)
    {
        return Repository.FirstOrDefault(packageGameTypeTimeId)?.AppUserId == userId;
    }

    public PackageGameTypeTime UpdatePackageGameTypeTimeWithUser(Guid userId, PackageGameTypeTime packageGameTypeTime)
    {
        var dto = Mapper.Map(packageGameTypeTime)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Update(dto))!;
    }
}
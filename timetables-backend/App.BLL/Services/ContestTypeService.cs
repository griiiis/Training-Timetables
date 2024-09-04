using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class ContestTypeService : BaseEntityService<App.DAL.DTO.ContestType, App.BLL.DTO.ContestType, IContestTypeRepository, IAppUnitOfWork>, IContestTypeService
{
    public ContestTypeService(IAppUnitOfWork uow, IContestTypeRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.ContestType, App.BLL.DTO.ContestType>(mapper))
    {
    }
    
    public App.BLL.DTO.ContestType AddContestTypeWithUser(Guid userId, ContestType contestType)
    {
        var dto = Mapper.Map(contestType)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Add(dto))!;
    }

    public bool IsContestTypeOwnedByUser(Guid userId, Guid contestTypeId)
    {
        return Repository.FirstOrDefault(contestTypeId)?.AppUserId == userId;
    }

    public ContestType UpdateContestTypeWithUser(Guid userId, ContestType contestType)
    {
        var dto = Mapper.Map(contestType)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Update(dto))!;
    }
}
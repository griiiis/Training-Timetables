using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class CourtService : BaseEntityService<App.DAL.DTO.Court, App.BLL.DTO.Court, ICourtRepository, IAppUnitOfWork>, ICourtService
{
    public CourtService(IAppUnitOfWork uow, ICourtRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.Court, App.BLL.DTO.Court>(mapper))
    {
    }
    public async Task<IEnumerable<Court>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId)).Select(de => Mapper.Map(de));
    }
    public async Task<IEnumerable<Court>> GetAllCurrentContestAsync(Guid contestId = default, bool noTracking = true)
    {
        return (await Repository.GetAllCurrentContestAsync(contestId)).Select(de => Mapper.Map(de));
    }
    
    public App.BLL.DTO.Court AddCourtWithUser(Guid userId, Court court)
    {
        var dto = Mapper.Map(court)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Add(dto))!;
    }

    public bool IsCourtOwnedByUser(Guid userId, Guid courtId)
    {
        return Repository.FirstOrDefault(courtId)?.AppUserId == userId;
    }

    public Court UpdateCourtWithUser(Guid userId, Court court)
    {
        var dto = Mapper.Map(court)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Update(dto))!;
    }
}
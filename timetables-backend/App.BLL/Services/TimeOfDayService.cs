using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class TimeOfDayService : BaseEntityService<App.DAL.DTO.TimeOfDay, App.BLL.DTO.TimeOfDay, ITimeOfDayRepository, IAppUnitOfWork>, ITimeOfDayService
{
    public TimeOfDayService(IAppUnitOfWork uow, ITimeOfDayRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.TimeOfDay, App.BLL.DTO.TimeOfDay>(mapper))
    {
    }

    public TimeOfDay AddTimeOfDayWithUser(Guid userId, TimeOfDay timeOfDay)
    {
        var dto = Mapper.Map(timeOfDay)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Add(dto))!;
    }

    public bool IsTimeOfDayOwnedByUser(Guid userId, Guid timeOfDayId)
    {
        return Repository.FirstOrDefault(timeOfDayId)?.AppUserId == userId;
    }

    public TimeOfDay UpdateTimeOfDayWithUser(Guid userId, TimeOfDay timeOfDay)
    {
        var dto = Mapper.Map(timeOfDay)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Update(dto))!;
    }

    public async Task<IEnumerable<TimeOfDay>> GetContestTimeOfDays(Guid contestId)
    {
        return (await Repository.GetContestTimeOfDays(contestId)).Select(e => Mapper.Map(e));
    }
    
}
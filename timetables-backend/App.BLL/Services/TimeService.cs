using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class TimeService : BaseEntityService<App.DAL.DTO.Time, App.BLL.DTO.Time, ITimeRepository, IAppUnitOfWork>, ITimeService
{
    public TimeService(IAppUnitOfWork uow, ITimeRepository repository, IMapper mapper) 
        : base(uow, repository, new BLLDalMapper<App.DAL.DTO.Time, App.BLL.DTO.Time>(mapper))
    {
    }
    
    public async Task<IEnumerable<Time>> GetAllCurrentContestAsync(Guid contestId = default, bool noTracking = true)
    {
        return (await Repository.GetAllCurrentContestAsync(contestId)).Select(de => Mapper.Map(de));
    }

    public async Task<IEnumerable<Time>> GetAllCurrentContestWithTimesOfDayAsync(Guid contestId = default, bool noTracking = true)
    {
        return (await Repository.GetAllCurrentContestWithTimesOfDayAsync(contestId)).Select(de => Mapper.Map(de));
    }

    public App.BLL.DTO.Time AddTimeWithUser(Guid userId, Time time)
    {
        var dto = Mapper.Map(time)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Add(dto))!;
    }

    public bool IsTimeOwnedByUser(Guid userId, Guid timeId)
    {
        return Repository.FirstOrDefault(timeId)?.AppUserId == userId;
    }

    public Time UpdateTimeWithUser(Guid userId, Time time)
    {
        var dto = Mapper.Map(time)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Update(dto))!;
    }
}
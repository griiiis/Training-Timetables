using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class TimeOfDayRepository : BaseEntityRepository<APPDomain.TimeOfDay, DALDTO.TimeOfDay, AppDbContext>, ITimeOfDayRepository
{
    public TimeOfDayRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.TimeOfDay,DALDTO.TimeOfDay>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.TimeOfDay>> GetContestTimeOfDays(Guid contestId)
    {
        return (await CreateQuery().Where(e => e.ContestTimeOfDays!.Equals(contestId)).ToListAsync()).Select(e => Mapper.Map(e));
    }
}
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class GameTypeRepository : BaseEntityRepository<APPDomain.GameType, DALDTO.GameType, AppDbContext>, IGameTypeRepository
{
    public GameTypeRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.GameType,DALDTO.GameType>(mapper))
    {
    }
    
    public async Task<IEnumerable<DALDTO.GameType>> GetAllCurrentContestAsync(Guid contestId)
    {
        return (await CreateQuery()
            .Where(e => e.ContestGameTypes
                .Any(l => l.ContestId.Equals(contestId)))
            .ToListAsync()).Select(de => Mapper.Map(de));
    }
}
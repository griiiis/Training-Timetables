using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ContestTypeRepository : BaseEntityRepository<APPDomain.ContestType, DALDTO.ContestType, AppDbContext>, IContestTypeRepository {
    public ContestTypeRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.ContestType,DALDTO.ContestType>(mapper))
    {
    }
}
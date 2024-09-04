using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class LocationRepository : BaseEntityRepository<APPDomain.Location, DALDTO.Location, AppDbContext>, ILocationRepository
{
    public LocationRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DalDomainMapper<APPDomain.Location,DALDTO.Location>(mapper))
    {
    }
}
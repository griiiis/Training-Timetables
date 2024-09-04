using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ILocationRepository : IEntityRepository<DALDTO.Location>
{
    
}
using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IContestTypeRepository : IEntityRepository<DALDTO.ContestType>
{ 
}

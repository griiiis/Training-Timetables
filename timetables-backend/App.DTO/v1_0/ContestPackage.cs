using Base.Contracts.Domain;
using Base.Domain;

namespace App.DTO.v1_0;

public class ContestPackage : IDomainEntityId
{
    public Guid ContestId { get; set; }
    public Domain.Contest? Contest { get; set; }
    
    public Guid PackageGameTypeTimeId { get; set; }
    public Domain.PackageGameTypeTime? PackageGameTypeTime { get; set; }
    public Guid Id { get; set; }
}
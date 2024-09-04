using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class ContestPackage : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }
    public Guid PackageGameTypeTimeId { get; set; }
    public PackageGameTypeTime? PackageGameTypeTime { get; set; }
}
using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class ContestPackage : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid ContestId { get; set; }
    public Guid PackageGameTypeTimeId { get; set; }
    public PackageGameTypeTime? PackageGameTypeTime { get; set; }
}
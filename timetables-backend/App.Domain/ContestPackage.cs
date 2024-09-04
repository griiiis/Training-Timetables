using Base.Domain;

namespace App.Domain;

public class ContestPackage : BaseEntityId
{
    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }
    
    public Guid PackageGameTypeTimeId { get; set; }
    public PackageGameTypeTime? PackageGameTypeTime { get; set; }
}
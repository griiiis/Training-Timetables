using App.DTO.v1_0.Identity;
using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class UserContestPackage : IDomainEntityId
{
    public Guid PackageGameTypeTimeId { get; set; }
    public PackageGameTypeTime? PackageGameTypeTime { get; set; }
    
    //Võtsin ära AppUserId
    public AppUser? AppUser { get; set; }
    
    public decimal HoursAvailable { get; set; }

    public Guid ContestId { get; set; }

    public Guid TeamId { get; set; }
    public Team? Team { get; set; }
    public Guid LevelId { get; set; }
    public Level? Level { get; set; }
    public Guid Id { get; set; }
}
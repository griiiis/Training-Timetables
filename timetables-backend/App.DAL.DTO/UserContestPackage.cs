using App.DAL.DTO.Identity;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class UserContestPackage : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public decimal HoursAvailable { get; set; }
    public Guid TeamId { get; set; }
    public Team? Team { get; set; }
    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }
    public Guid LevelId { get; set; }
    public Level? Level { get; set; }
    public Guid PackageGameTypeTimeId { get; set; }
    public PackageGameTypeTime? PackageGameTypeTime { get; set; }
}
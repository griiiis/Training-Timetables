using App.DAL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.DAL.DTO;

public class GameType : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public LangStr GameTypeName { get; set; } = default!;
    public ICollection<Court>? Courts { get; set; }
    public ICollection<Game>? Games { get; set; }
    public ICollection<Team>? Teams { get; set; }
    public ICollection<RolePreference>? RolePreferences { get; set; }
    public ICollection<PackageGameTypeTime>? PackageGameTypeTimes { get; set; }
    public ICollection<ContestGameType>? ContestGameTypes { get; set; }
}
using App.DAL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.DAL.DTO;

public class Level : IDomainEntityId   
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public LangStr Title { get; set; } = default!;
    public LangStr Description { get; set; } = default!;
    public ICollection<Game>? Games { get; set; }
    public ICollection<RolePreference>? RolePreferences { get; set; }
    public ICollection<Team>? Teams { get; set; }
    public ICollection<ContestLevel>? ContestLevels { get; set; }
}

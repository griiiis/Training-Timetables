using App.DAL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.DAL.DTO;

public class Location : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public LangStr LocationName { get; set; } = default!;
    public LangStr State { get; set; } = default!;
    public LangStr Country { get; set; } = default!;
    public ICollection<Contest>? Contests { get; set; }
    public ICollection<Court>? Courts { get; set; }
}
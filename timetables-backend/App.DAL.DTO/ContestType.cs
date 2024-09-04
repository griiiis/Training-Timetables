using App.DAL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.DAL.DTO;

public class ContestType : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public LangStr ContestTypeName { get; set; } = default!;
    public LangStr Description { get; set; } = default!;
    public ICollection<Contest>? Contests { get; set; }
}
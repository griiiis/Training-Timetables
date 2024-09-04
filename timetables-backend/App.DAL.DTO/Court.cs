using App.DAL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.DAL.DTO;

public class Court : IDomainEntityId   
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public LangStr CourtName { get; set; } = default!;
    public Guid GameTypeId { get; set; }
    public GameType? GameType { get; set; }
    public Guid LocationId { get; set; }
    public Location? Location { get; set; }
    public ICollection<Game>? Games { get; set; }
}
using App.DAL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.DAL.DTO;

public class PackageGameTypeTime : IDomainEntityId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public Guid Id { get; set; }
    public LangStr PackageGtName { get; set; } = default!;
    public Guid GameTypeId { get; set; }
    public GameType? GameType { get; set; }
    public decimal Times { get; set; }
}
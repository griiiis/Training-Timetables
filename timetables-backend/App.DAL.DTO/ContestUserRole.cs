using App.DAL.DTO.Identity;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class ContestUserRole : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid ContestRoleId { get; set; }
    public ContestRole? ContestRole { get; set; }

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}
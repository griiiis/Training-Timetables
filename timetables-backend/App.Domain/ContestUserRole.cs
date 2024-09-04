using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class ContestUserRole : BaseEntityId
{
    public Guid ContestRoleId { get; set; }
    public ContestRole? ContestRole { get; set; }

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}
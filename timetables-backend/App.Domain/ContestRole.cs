using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class ContestRole : BaseEntityId
{
    public string ContestRoleName { get; set; } = default!;

    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }

    public ICollection<ContestUserRole>? ContestUserRoles { get; set; }

}
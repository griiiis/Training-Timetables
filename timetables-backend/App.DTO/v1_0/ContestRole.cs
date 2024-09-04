using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class ContestRole : IDomainEntityId
{
    public Guid Id { get; set; }
    public string ContestRoleName { get; set; } = default!;

    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }

    public ICollection<ContestUserRole>? ContestUserRoles { get; set; }

}
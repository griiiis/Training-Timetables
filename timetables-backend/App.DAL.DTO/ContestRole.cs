using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class ContestRole : IDomainEntityId
{
    public Guid Id { get; set; }
    public string ContestRoleName { get; set; } = default!;

    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }

    public ICollection<ContestUserRole>? ContestUserRoles { get; set; }

}
using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class ContestUserRole : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid ContestRoleId { get; set; }
    public ContestRole? ContestRole { get; set; }
}
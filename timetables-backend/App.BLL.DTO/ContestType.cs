using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class ContestType : IDomainEntityId
{
    public Guid Id { get; set; }
    public string ContestTypeName { get; set; } = default!;
    public string Description { get; set; } = default!;
}
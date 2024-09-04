using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class Location : IDomainEntityId
{
    public string LocationName { get; set; } = default!;
    public string? State { get; set; }
    public string Country { get; set; } = default!;
    public Guid Id { get; set; }
}
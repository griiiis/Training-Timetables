using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class Level : IDomainEntityId   
{
    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
    public Guid Id { get; set; }
}

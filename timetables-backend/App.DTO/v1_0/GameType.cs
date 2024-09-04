using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class GameType : IDomainEntityId
{
    public string GameTypeName { get; set; } = default!;
    public Guid Id { get; set; }
}
using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class ContestGameType : IDomainEntityId
{
    public Guid ContestId { get; set; }
    
    public Guid GameTypeId { get; set; }
    public GameType? GameType { get; set; }
    public Guid Id { get; set; }
}
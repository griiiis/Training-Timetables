using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class ContestTime : IDomainEntityId
{
    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }
    
    public Guid TimeId { get; set; }
    public Time? Time { get; set; }
    public Guid Id { get; set; }
}
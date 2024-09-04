using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class ContestTime : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid ContestId { get; set; }
    public Guid TimeId { get; set; }
    public Time? Time { get; set; }
}
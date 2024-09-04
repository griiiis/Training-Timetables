using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class ContestGameType : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid ContestId { get; set; }
    public Guid GameTypeId { get; set; }
    public GameType? GameType { get; set; }
}
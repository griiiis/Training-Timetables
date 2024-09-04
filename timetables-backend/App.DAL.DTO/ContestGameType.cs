using Base.Contracts.Domain;
using Base.Domain;

namespace App.DAL.DTO;

public class ContestGameType : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }
    public Guid GameTypeId { get; set; }
    public GameType? GameType { get; set; }
}
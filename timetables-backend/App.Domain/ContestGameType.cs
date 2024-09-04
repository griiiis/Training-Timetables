using Base.Domain;

namespace App.Domain;

public class ContestGameType : BaseEntityId
{
    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }
    
    public Guid GameTypeId { get; set; }
    public GameType? GameType { get; set; }
}
using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class RolePreference : IDomainEntityId
{
    public Guid LevelId { get; set; }
    public Level? Level { get; set; }
    
    public Guid GameTypeId { get; set; }
    public GameType? GameType { get; set; }
    public Guid ContestId { get; set; }

    public Guid Id { get; set; }
}
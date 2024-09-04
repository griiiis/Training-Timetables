using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class  ContestLevel : IDomainEntityId
{
    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }
    
    public Guid LevelId { get; set; }
    public Level? Level { get; set; }

    public Guid Id { get; set; }
}
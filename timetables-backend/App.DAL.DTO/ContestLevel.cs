using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class  ContestLevel : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }
    public Guid LevelId { get; set; }
    public Level? Level { get; set; }
}
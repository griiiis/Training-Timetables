using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class  ContestLevel : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid ContestId { get; set; }
    public Guid LevelId { get; set; }
    public Level? Level { get; set; }
}
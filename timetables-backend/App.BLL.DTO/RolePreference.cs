using App.BLL.DTO.Identity;
using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class RolePreference : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid LevelId { get; set; }
    public Level? Level { get; set; }
    public Guid GameTypeId { get; set; }
    public GameType? GameType { get; set; }
    public Guid ContestId { get; set; }
    public int GamesTrainedHourly { get; set; }
}
using App.DAL.DTO.Identity;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Time : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public TimeOnly From { get; set; }
    public TimeOnly Until { get; set; }
    public Guid TimeOfDayId { get; set; }
    public TimeOfDay? TimeOfDay { get; set; }
    public ICollection<ContestTime>? ContestTimes { get; set; }
}
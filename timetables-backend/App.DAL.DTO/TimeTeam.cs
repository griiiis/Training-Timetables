using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class TimeTeam : IDomainEntityId        
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public Team? Team { get; set; }
    public Guid TimeOfDayId { get; set; }
    public TimeOfDay? TimeOfDay { get; set; }
    public DateOnly Day { get; set; }
}
using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class TimeTeam : IDomainEntityId        
{
    public Guid TeamId { get; set; }
    public Team? Team { get; set; }
    
    public Guid TimeOfDayId { get; set; }
    public TimeOfDay? TimeOfDay { get; set; }

    public DateOnly Day { get; set; }
    public Guid Id { get; set; }
}
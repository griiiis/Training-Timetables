using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class TimeTeam : IDomainEntityId        
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public Guid TimeOfDayId { get; set; }
    public DateOnly Day { get; set; }
}
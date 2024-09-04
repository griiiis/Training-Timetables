using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class Time : IDomainEntityId
{
    public TimeOnly From { get; set; }
    public TimeOnly Until { get; set; }
    
    public Guid TimeOfDayId { get; set; }
    public TimeOfDay? TimeOfDay { get; set; }

    public Guid Id { get; set; }
}
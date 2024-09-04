using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class TimeOfDay : IDomainEntityId
{
    public string TimeOfDayName { get; set; } = default!;
    public Guid Id { get; set; }
}
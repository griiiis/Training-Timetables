using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class ContestTimeOfDay : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }
    
    public Guid TimeOfDayId { get; set; }
    public TimeOfDay? TimeOfDay { get; set; }
}
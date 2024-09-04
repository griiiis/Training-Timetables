using Base.Domain;

namespace App.Domain;

public class ContestTimeOfDay : BaseEntityId
{
    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }
    
    public Guid TimeOfDayId { get; set; }
    public TimeOfDay? TimeOfDay { get; set; }
}
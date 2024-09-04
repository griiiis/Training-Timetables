using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;

public class TimeOfDay : IDomainEntityId
{
    public Guid Id { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.TimeOfDay), Name = nameof(TimeOfDayName))]
    public LangStr TimeOfDayName { get; set; } = default!;
}
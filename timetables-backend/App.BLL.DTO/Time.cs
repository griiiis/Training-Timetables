using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class Time : IDomainEntityId
{
    public Guid Id { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Time), Name = nameof(From))]
    public TimeOnly From { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Time), Name = nameof(Until))]
    public TimeOnly Until { get; set; }
    public Guid TimeOfDayId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Time), Name = nameof(TimeOfDayId))]
    public TimeOfDay? TimeOfDay { get; set; }
}
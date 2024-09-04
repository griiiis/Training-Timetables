using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;

public class Location : IDomainEntityId
{
    public Guid Id { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Location), Name = nameof(LocationName))]
    public LangStr LocationName { get; set; } = default!;
    [Display(ResourceType = typeof(App.Resources.Domain.Location), Name = nameof(State))]
    public LangStr? State { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.Location), Name = nameof(Country))]
    public LangStr Country { get; set; } = default!;
    
}
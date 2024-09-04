using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;

public class Level : IDomainEntityId   
{
    [Display(ResourceType = typeof(App.Resources.Domain.Level), Name = nameof(Title))]
    public LangStr Title { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.Domain.Level), Name = nameof(Description))]
    public LangStr Description { get; set; } = default!;
    public Guid Id { get; set; }
}

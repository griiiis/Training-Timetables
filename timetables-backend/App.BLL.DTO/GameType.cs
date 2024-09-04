using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;

public class GameType : IDomainEntityId
{
    public Guid Id { get; set; }
    [Display(ResourceType = typeof(App.Resources.Domain.GameType), Name = nameof(GameTypeName))]
    public LangStr GameTypeName { get; set; } = default!;
}
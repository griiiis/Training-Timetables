using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class  ContestLevel : BaseEntityId
{
    public Guid ContestId { get; set; }
    public Contest? Contest { get; set; }
    
    public Guid LevelId { get; set; }
    public Level? Level { get; set; }

}
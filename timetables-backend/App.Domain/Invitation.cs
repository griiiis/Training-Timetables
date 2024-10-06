using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Enums;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Invitation : BaseEntityId
{
    public Guid SenderContestPackageId { get; set; } = default!;
    public UserContestPackage? SenderContestPackage { get; set; }
    
    public Guid GetterContestPackageId { get; set; } = default!;
    public UserContestPackage? GetterContestPackage { get; set; }

    public Guid TeamId { get; set; }
    public Team? Team { get; set; }

    public EStatus Status { get; set; }
    public DateTime TimeInvited { get; set; }
    public DateTime TimeAnswered { get; set; }
}
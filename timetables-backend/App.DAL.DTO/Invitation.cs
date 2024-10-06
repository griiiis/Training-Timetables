﻿using App.DAL.DTO.Enums;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Invitation : IDomainEntityId
{
    public Guid Id { get; set; }
    
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
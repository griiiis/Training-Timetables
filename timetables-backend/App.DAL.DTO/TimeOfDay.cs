﻿using App.DAL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.DAL.DTO;

public class TimeOfDay : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public LangStr TimeOfDayName { get; set; } = default!;
    public ICollection<Time>? Times { get; set; }
    public ICollection<TimeTeam>? TimeTeams { get; set; }
}
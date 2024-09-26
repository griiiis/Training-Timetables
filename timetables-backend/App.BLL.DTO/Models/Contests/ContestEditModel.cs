﻿namespace App.BLL.DTO.Models.Contests;

public class ContestEditModel
{
    public Contest Contest { get; set; } = default!;
    public List<Guid>? LevelIds { get; set; }
    public List<Guid>? TimesIds { get; set; }
    public List<Guid>? PackagesIds { get; set; }
}
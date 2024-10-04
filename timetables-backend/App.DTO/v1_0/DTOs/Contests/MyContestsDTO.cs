namespace App.DTO.v1_0.DTOs.Contests;


public record MyContestsDTO
{
    public List<UserContestsDTO>? CurrentContestsDTO { get; set; }
    public List<UserContestsDTO>? ComingContestsDTO { get; set; }
}

public record UserContestsDTO
{
    public Guid ContestId { get; set; }
    public string ContestName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int TotalHours { get; set; }
    public DateTime From { get; set; }
    public DateTime Until { get; set; }
    public string LocationName { get; set; } = default!;
    public string ContestTypeName { get; set; } = default!;

    public bool AnyGames { get; set; }

    public Guid TeamId { get; set; }

    public bool IfTrainer { get; set; }
    
    // For team member
    public List<UserPackagesDTO> PackagesDTOs { get; set; } = default!;
    
    // For participant
    public string? LevelTitle { get; set; }
    public string? GameTypeName { get; set; }
    public string? PackageName { get; set; }
    
    
    // For trainer, what different game types they teach
    public List<GameTypesDTO>? GameTypesDTOs { get; set; }
    
    // For trainer, Role preferences
    public List<RolePreferenceDTO>? RolePreferenceDTOs { get; set; }
}

public record UserPackagesDTO
{
    public Guid PackageId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}

public record GameTypesDTO
{
    public Guid GameTypeId { get; set; }
    public string GameTypeName { get; set; } = default!;
}

public record RolePreferenceDTO
{
    public Guid GameTypeId { get; set; }
    public string GameTypeName { get; set; } = default!;
    public string LevelTitle { get; set; } = default!;
}
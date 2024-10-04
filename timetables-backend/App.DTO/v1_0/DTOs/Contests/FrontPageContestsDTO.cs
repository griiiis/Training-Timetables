namespace App.DTO.v1_0.DTOs.Contests;


public record FrontPageContestsDTO
{
    public List<FrontPageContestDTO>? CurrentContestsDTO { get; set; }
    public List<FrontPageContestDTO>? ComingContestsDTO { get; set; }
}

public record FrontPageContestDTO
{
    public Guid Id { get; set; }
    public string ContestName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int TotalHours { get; set; }
    public DateTime From { get; set; }
    public DateTime Until { get; set; }
    public string LocationName { get; set; } = default!;
    public string ContestTypeName { get; set; } = default!;

    public int NumberOfParticipants { get; set; }

    public List<string> ContestGameTypes { get; set; } = default!;

}
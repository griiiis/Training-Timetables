namespace App.DTO.v1_0.DTOs.Contests;

public record InformationContestDTO
{
    public Guid Id { get; set; }
    public string ContestName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int TotalHours { get; set; }
    public DateTime From { get; set; }
    public DateTime Until { get; set; }
    public string LocationName { get; set; } = default!;
    public string ContestTypeName { get; set; } = default!;
}
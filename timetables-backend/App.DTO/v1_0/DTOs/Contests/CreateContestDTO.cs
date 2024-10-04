namespace App.DTO.v1_0.DTOs.Contests;

public record CreateContestDTO
{
    public string ContestName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int TotalHours { get; set; }
    public DateTime From { get; set; }
    public DateTime Until { get; set; }
    public Guid LocationId { get; set; }
    public Guid ContestTypeId { get; set; }
    
    public List<Guid>? SelectedLevelIds { get; set; }
    public List<Guid>? SelectedTimesIds { get; set; }
    public List<Guid>? SelectedPackagesIds { get; set; }
}
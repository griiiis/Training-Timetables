namespace App.BLL.DTO.DTOs.Contests;

public class EditContestDTO
{
    public Guid Id { get; set; }
    public string ContestName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int TotalHours { get; set; }
    public DateTime From { get; set; }
    public DateTime Until { get; set; }
    public Guid LocationId { get; set; }
    public Guid ContestTypeId { get; set; }
    public List<Guid>? LevelIds { get; set; }
    public List<Guid>? TimesIds { get; set; }
    public List<Guid>? PackagesIds { get; set; }
}
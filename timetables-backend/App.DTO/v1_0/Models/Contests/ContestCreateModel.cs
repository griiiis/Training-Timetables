namespace App.DTO.v1_0.Models.Contests;

public class ContestCreateModel
{
    public Contest Contest { get; set; } = default!;
    public List<Guid>? SelectedLevelIds { get; set; }
    public List<Guid>? SelectedTimesIds { get; set; }
    public List<Guid>? SelectedPackagesIds { get; set; }
}
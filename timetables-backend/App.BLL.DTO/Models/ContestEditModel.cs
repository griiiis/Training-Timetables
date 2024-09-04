namespace App.BLL.DTO.Models;

public class ContestEditModel
{
    public Contest Contest { get; set; } = default!;
    public List<ContestType>? ContestTypeList { get; set; }
    public List<Location>? LocationList { get; set; }
    public List<Level>? LevelList { get; set; }
    public List<Time>? TimesList { get; set; }
    public List<PackageGameTypeTime>? PackagesList { get; set; }
    
    public List<Level>? PreviousLevels { get; set; }
    public List<Time>? PreviousTimes { get; set; }
    public List<PackageGameTypeTime>? PreviousPackages { get; set; }
    
    public List<Guid>? SelectedLevelIds { get; set; }
    public List<Guid>? SelectedTimesIds { get; set; }
    public List<Guid>? SelectedPackagesIds { get; set; }
}
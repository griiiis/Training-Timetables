using App.BLL.DTO;

namespace WebApp.ViewModels;

public class ContestMyContestsViewModel
{
    public List<ContestViewModel> ComingContests { get; set; } = default!;
    public List<ContestViewModel> CurrentContests { get; set; } = default!;
    public List<Contest> EndedContests { get; set; } = default!;
    public List<RolePreference>? RolePreferences { get; set; }
    
    
    public class ContestViewModel
    {
        public Contest Contest { get; set; } = default!;
        public bool AnyGames { get; set; }
        public bool IfTrainer { get; set; }
        public Guid UserId { get; set; }
        public UserContestPackage UserContestPackage { get; set; } = default!;
        public List<UserContestPackage> UserContestPackages { get; set; } = default!;
        public List<GameType> GameTypes { get; set; } = default!;
        public List<string>? SelectedTitles { get; set; }
        public Level Level { get; set; } = default!;
        public GameType GameType { get; set; } = default!;
        public PackageGameTypeTime PackageGameTypeTime { get; set; } = default!;
    }
}
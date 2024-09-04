using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using Game = App.DAL.DTO.Game;

namespace WebApp.ViewModels;

public class ContestIndexViewModel
{
    public List<ContestViewModel>? ComingContests { get; set; } = default!;
    public List<ContestViewModel>? CurrentContests { get; set; } = default!;
    
    [BindProperty(SupportsGet = true)] public string? Search { get; set; }
    [BindProperty(SupportsGet = true)] public bool Location { get; set; }
    [BindProperty(SupportsGet = true)] public bool ContestType { get; set; }
    [BindProperty(SupportsGet = true)] public bool GameType { get; set; }
    [BindProperty(SupportsGet = true)] public List<Contest>? SearchedContests { get; set; } = default!;

    public class ContestViewModel
    {
        public Contest Contest { get; set; } = default!;
        public List<GameType> GameTypes { get; set; } = default!;
        public int NumberOfParticipants { get; set; }
        public bool ifAlreadyJoined { get; set; }
    }
}
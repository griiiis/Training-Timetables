using AutoMapper;

namespace App.BLL;

public class AutoMapperProfile : Profile
{ 
    public AutoMapperProfile()
    {
        CreateMap<App.DAL.DTO.Contest, App.BLL.DTO.Contest>().ReverseMap();
        CreateMap<App.DAL.DTO.ContestType, App.BLL.DTO.ContestType>().ReverseMap();
        CreateMap<App.DAL.DTO.Court, App.BLL.DTO.Court>().ReverseMap();
        CreateMap<App.DAL.DTO.Game, App.BLL.DTO.Game>().ReverseMap();
        CreateMap<App.DAL.DTO.GameType, App.BLL.DTO.GameType>().ReverseMap();
        CreateMap<App.DAL.DTO.Level, App.BLL.DTO.Level>().ReverseMap();
        CreateMap<App.DAL.DTO.Location, App.BLL.DTO.Location>().ReverseMap();
        CreateMap<App.DAL.DTO.PackageGameTypeTime, App.BLL.DTO.PackageGameTypeTime>().ReverseMap();
        CreateMap<App.DAL.DTO.RolePreference, App.BLL.DTO.RolePreference>().ReverseMap();
        CreateMap<App.DAL.DTO.Team, App.BLL.DTO.Team>().ReverseMap();
        CreateMap<App.DAL.DTO.TeamGame, App.BLL.DTO.TeamGame>().ReverseMap();
        CreateMap<App.DAL.DTO.Time, App.BLL.DTO.Time>().ReverseMap();
        CreateMap<App.DAL.DTO.TimeOfDay, App.BLL.DTO.TimeOfDay>().ReverseMap();
        CreateMap<App.DAL.DTO.TimeTeam, App.BLL.DTO.TimeTeam>().ReverseMap();
        CreateMap<App.DAL.DTO.UserContestPackage, App.BLL.DTO.UserContestPackage>().ReverseMap();
        CreateMap<App.DAL.DTO.Identity.AppUser, App.BLL.DTO.Identity.AppUser>().ReverseMap();
        CreateMap<App.DAL.DTO.ContestLevel, App.BLL.DTO.ContestLevel>().ReverseMap();
        CreateMap<App.DAL.DTO.ContestGameType, App.BLL.DTO.ContestGameType>().ReverseMap();
        CreateMap<App.DAL.DTO.ContestTime, App.BLL.DTO.ContestTime>().ReverseMap();
        CreateMap<App.DAL.DTO.ContestPackage, App.BLL.DTO.ContestPackage>().ReverseMap();
        CreateMap<App.DAL.DTO.ContestRole, App.BLL.DTO.ContestRole>().ReverseMap();
        CreateMap<App.DAL.DTO.ContestUserRole, App.BLL.DTO.ContestUserRole>().ReverseMap();
    }
    
}
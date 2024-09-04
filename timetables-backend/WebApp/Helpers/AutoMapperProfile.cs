using AutoMapper;

namespace WebApp.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.DTO.v1_0.Identity.AppUser, App.BLL.DTO.Identity.AppUser>().ReverseMap();
        CreateMap<App.DTO.v1_0.Contest, App.BLL.DTO.Contest>().ReverseMap();
        CreateMap<App.DTO.v1_0.ContestType, App.BLL.DTO.ContestType>().ReverseMap();
        CreateMap<App.DTO.v1_0.Court, App.BLL.DTO.Court>().ReverseMap();
        CreateMap<App.DTO.v1_0.Game, App.BLL.DTO.Game>().ReverseMap();
        CreateMap<App.DTO.v1_0.GameType, App.BLL.DTO.GameType>().ReverseMap();
        CreateMap<App.DTO.v1_0.Level, App.BLL.DTO.Level>().ReverseMap();
        CreateMap<App.DTO.v1_0.Location, App.BLL.DTO.Location>().ReverseMap();
        CreateMap<App.DTO.v1_0.PackageGameTypeTime, App.BLL.DTO.PackageGameTypeTime>().ReverseMap();
        CreateMap<App.DTO.v1_0.RolePreference, App.BLL.DTO.RolePreference>().ReverseMap();
        CreateMap<App.DTO.v1_0.Team, App.BLL.DTO.Team>().ReverseMap();
        CreateMap<App.DTO.v1_0.TeamGame, App.BLL.DTO.TeamGame>().ReverseMap();
        CreateMap<App.DTO.v1_0.Time, App.BLL.DTO.Time>().ReverseMap();
        CreateMap<App.DTO.v1_0.TimeOfDay, App.BLL.DTO.TimeOfDay>().ReverseMap();
        CreateMap<App.DTO.v1_0.TimeTeam, App.BLL.DTO.TimeTeam>().ReverseMap();
        CreateMap<App.DTO.v1_0.UserContestPackage, App.BLL.DTO.UserContestPackage>().ReverseMap();
        CreateMap<App.DTO.v1_0.ContestLevel, App.BLL.DTO.ContestLevel>().ReverseMap();
        CreateMap<App.DTO.v1_0.ContestGameType, App.BLL.DTO.ContestGameType>().ReverseMap();
        CreateMap<App.DTO.v1_0.ContestTime, App.BLL.DTO.ContestTime>().ReverseMap();
        CreateMap<App.DTO.v1_0.ContestPackage, App.BLL.DTO.ContestPackage>().ReverseMap();
        CreateMap<App.DTO.v1_0.ContestRole, App.BLL.DTO.ContestRole>().ReverseMap();
        CreateMap<App.DTO.v1_0.ContestUserRole, App.BLL.DTO.ContestUserRole>().ReverseMap();
        
    }
    
    
}
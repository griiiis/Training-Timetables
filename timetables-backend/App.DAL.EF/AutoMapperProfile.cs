using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.Domain.Contest, App.DAL.DTO.Contest>().ReverseMap();
        CreateMap<App.Domain.ContestType, App.DAL.DTO.ContestType>().ReverseMap();
        CreateMap<App.Domain.Court, App.DAL.DTO.Court>().ReverseMap();
        CreateMap<App.Domain.Game, App.DAL.DTO.Game>().ReverseMap();
        CreateMap<App.Domain.GameType, App.DAL.DTO.GameType>().ReverseMap();
        CreateMap<App.Domain.Level, App.DAL.DTO.Level>().ReverseMap();
        CreateMap<App.Domain.Location, App.DAL.DTO.Location>().ReverseMap();
        CreateMap<App.Domain.PackageGameTypeTime, App.DAL.DTO.PackageGameTypeTime>().ReverseMap();
        CreateMap<App.Domain.RolePreference, App.DAL.DTO.RolePreference>().ReverseMap();
        CreateMap<App.Domain.Team, App.DAL.DTO.Team>().ReverseMap();
        CreateMap<App.Domain.TeamGame, App.DAL.DTO.TeamGame>().ReverseMap();
        CreateMap<App.Domain.Time, App.DAL.DTO.Time>().ReverseMap();
        CreateMap<App.Domain.TimeOfDay, App.DAL.DTO.TimeOfDay>().ReverseMap();
        CreateMap<App.Domain.TimeTeam, App.DAL.DTO.TimeTeam>().ReverseMap();
        CreateMap<App.Domain.Enums.EGender, App.DAL.DTO.Enums.EGender>().ReverseMap();
        CreateMap<App.Domain.Identity.AppUser, App.DAL.DTO.Identity.AppUser>().ReverseMap();
        CreateMap<App.Domain.UserContestPackage, App.DAL.DTO.UserContestPackage>().ReverseMap();
        CreateMap<App.Domain.ContestLevel, App.DAL.DTO.ContestLevel>().ReverseMap();
        CreateMap<App.Domain.ContestGameType, App.DAL.DTO.ContestGameType>().ReverseMap();
        CreateMap<App.Domain.ContestTime, App.DAL.DTO.ContestTime>().ReverseMap();
        CreateMap<App.Domain.ContestPackage, App.DAL.DTO.ContestPackage>().ReverseMap();
        CreateMap<App.Domain.ContestRole, App.DAL.DTO.ContestRole>().ReverseMap();
        CreateMap<App.Domain.ContestUserRole, App.DAL.DTO.ContestUserRole>().ReverseMap();
    }

}
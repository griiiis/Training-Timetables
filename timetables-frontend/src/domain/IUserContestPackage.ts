
import { ILevel } from "./ILevel"
import { IPackageGameTypeTime } from "./IPackageGameTypeTime"
import { ITeam } from "./ITeam"
import { IAppUser } from "./Identity/IAppUser"

export interface IUserContestPackage {
        "id": string,
        "packageGameTypeTimeId": string,
        "hoursAvailable": number,
        "contestId": string,
        "levelId": string,
        "appUser": IAppUser,
        "packageGameTypeTime": IPackageGameTypeTime
        "level": ILevel,
        "team": ITeam,
        "teamId": string
}
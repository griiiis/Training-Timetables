import { ILevel } from "./ILevel"

export interface IRolePreference {
        "levelId": string,
        "level" : ILevel
        "gameTypeId": string,
        "userRoleId" : string,
        "id": string,
        "contestId" : string
}
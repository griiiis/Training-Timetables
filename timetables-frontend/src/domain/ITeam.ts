import { ITeamGame } from "./ITeamGame"
import { IUserContestPackage } from "./IUserContestPackage"

export interface ITeam {
        "teamName": string,
        "levelId": string,
        "gameTypeId": string
        "id": string,
        "level": {
                "title": string,
                "description": string,
                "id": string
        },
        "gameType": {
                "gameTypeName": string,
                "id": string
        },
        "userContestPackages": [IUserContestPackage],
        "teamGames" : [ITeamGame]
}
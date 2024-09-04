import { ITeamGame } from "./ITeamGame"

export interface IGame {
        "title" : string,
        "from": string,
        "until": string,
        "contestId": string,
        "courtId": string,
        "gameTypeId": string,
        "timeId": string,
        "levelId": string,
        "id": string
        "court": {
                "courtName": "string",
        },
        "level": {
                "title": "string",
        }
}
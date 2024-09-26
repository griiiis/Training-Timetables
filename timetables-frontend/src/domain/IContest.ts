import { IContestGameType } from "./IContestGameType";
import { IContestType } from "./IContestType";
import { ILocation } from "./ILocation";

export interface IContest {
        "contestName": string,
        "id": string,
        "description": string,
        "from": string,
        "until": string,
        "totalHours": number,
        "contestType": IContestType,
        "location": ILocation,
        "contestGameTypes": [IContestGameType],
}
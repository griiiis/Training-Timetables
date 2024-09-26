import { IContest } from "@/domain/IContest";
import { IContestType } from "@/domain/IContestType";
import { ILocation } from "@/domain/ILocation";

export interface IContestEditModel {
    "contest": {
        "contestName": string,
        "id": string,
        "description": string,
        "from": string,
        "until": string,
        "totalHours": number,
        "contestTypeId": string,
        "locationId": string,
    },
    
    "levelIds": string[],
    "timesIds": string[],
    "packagesIds": string[],
}
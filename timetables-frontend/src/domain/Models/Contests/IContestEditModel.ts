import { IContestType } from "../../IContestType"
import { ILevel } from "../../ILevel"
import { ILocation } from "../../ILocation"
import { IPackageGameTypeTime } from "../../IPackageGameTypeTime"
import { ITime } from "../../ITime"

export interface IContestEditModel {
    "contest": {
        "contestName": string,
        "id": string,
        "description": string,
        "from": string,
        "until": string,
        "totalHours": number,
        "contestType": IContestType,
        "location": ILocation,
    },

    "contestTypeList": IContestType[],
    "locationList": ILocation[],
    "timesList": ITime[],
    "levelList": ILevel[],
    "packagesList": IPackageGameTypeTime[],

    "previousLevels": ILevel[],
    "previousTimes": ITime[],
    "previousPackages": IPackageGameTypeTime[],
    
    "selectedLevelIds": string[],
    "selectedTimesIds": string[],
    "selectedPackagesIds": string[],
}
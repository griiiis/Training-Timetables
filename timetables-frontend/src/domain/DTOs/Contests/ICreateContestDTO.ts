export interface ICreateContestDTO {
    "contestName": string,
    "description": string,
    "from": string,
    "until": string,
    "totalHours": number,
    "contestTypeId": string,
    "locationId": string,
    "selectedLevelIds": string[],
    "selectedTimesIds": string[],
    "selectedPackagesIds": string[],
}
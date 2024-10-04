export interface IEditContestDTO {
    "id": string,
    "contestName": string,
    "description": string,
    "from": string,
    "until": string,
    "totalHours": number,
    "contestTypeId": string,
    "locationId": string,
    "levelIds": string[],
    "timesIds": string[],
    "packagesIds": string[],
}
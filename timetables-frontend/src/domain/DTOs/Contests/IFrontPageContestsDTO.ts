export interface IFrontPageContestsDTO {
    "currentContestsDTO": IFrontPageContestDTO[],
    "comingContestsDTO": IFrontPageContestDTO[],
}

interface IFrontPageContestDTO {
    "id" : string,
    "contestName" : string,
    "description" : string,
    "from" : string,
    "until" : string,
    "totalHours" : number,
    "locationName" : string,
    "contestTypeName" : string,
    "numberOfParticipants" : number,
    "contestGameTypes" : string[],
}
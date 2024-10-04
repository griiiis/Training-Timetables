export interface IMyContestsDTO {
    "currentContestsDTO": IUserContestsDTO[],
    "comingContestsDTO": IUserContestsDTO[]
}

interface IUserContestsDTO {
    "contestId" : string,
    "contestName" : string,
    "description" : string,
    "from" : string,
    "until" : string,
    "totalHours" : number,
    "locationName" : string,
    "contestTypeName" : string,
    "anyGames" : boolean,
    "teamId" : string,
    "ifTrainer" : boolean,
    "packagesDTOs" :[IUserPackagesDTO]
    "levelTitle" : string,
    "gameTypeName" : string,
    "packageName" : string,
    "gameTypesDTOs" : [IGameTypesDTO],
    "rolePreferencesDTOs" : [IRolePreferencesDTO]
}

interface IUserPackagesDTO {
    "packageId" : string,
    "firstName" : string,
    "lastName" : string
}

interface IGameTypesDTO {
    "gameTypeId" : string,
    "gameTypeName" : string
}

interface IRolePreferencesDTO {
    "gameTypeId" : string,
    "gameTypeName" : string,
    "levelTitle" : string,
}
export interface IUserTeam {
        "userContestPackageId": string,
        "userContestPackage": {
                "appUserId" : string,
                "appUser": {
                        "firstName": string,
                        "lastName": string
                },
                "contestId": string,

        },
        "teamId": string,
        "id": string,
        "team": {
                "teamName": string,
                "levelId": string,
                "level": {
                        "title": string,
                        "description": string,
                },
                "gameTypeId": string,
                "gameType": {
                        "gameTypeName": string,
                },
                "teamGames": [
                        {
                        "gameId": string,
                        }],
        },
}
export interface ICourt {
        "courtName": string,
        "locationId": string,
        "id": string,
        "location": {
                "locationName": string,
                "state": string,
                "country": string,
                "id": string
        },
        "gameTypeId": string,
        "gameType": {
                "gameTypeName": string,
                "id": string
        },
}
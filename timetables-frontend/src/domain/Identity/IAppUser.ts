import { IRolePreference } from "../IRolePreference";

export interface IAppUser {
    "firstName" : string,
    "lastName" : string,
    "email": string,
    "rolePreferences" : [IRolePreference]
}
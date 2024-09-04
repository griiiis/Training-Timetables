import { IAppRole } from "../Identity/IAppRole";
import { IAppUser } from "../Identity/IAppUser";

export interface IAppUserModel {

    "appUser" : IAppUser,
    "roleSelectList" : Array<IAppRole>,
    "selectedRoleId" : string
}
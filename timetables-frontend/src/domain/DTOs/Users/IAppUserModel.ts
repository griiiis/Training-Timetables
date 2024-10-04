import { IAppRole } from "@/domain/Identity/IAppRole";
import { IAppUser } from "@/domain/Identity/IAppUser";


export interface IAppUserModel {

    "appUser" : IAppUser,
    "roleSelectList" : Array<IAppRole>,
    "selectedRoleId" : string
}
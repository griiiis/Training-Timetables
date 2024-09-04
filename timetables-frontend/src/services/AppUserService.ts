import BaseService from "./BaseService";
import { IResultObject } from "./IResultObject";
import { IUserInfo } from "@/state/AppContext";
import { IAppUserModel } from "@/domain/Models/IAppUserModel";
import { IAppUser } from "@/domain/Identity/IAppUser";


export default class AppUserService extends BaseService {
    private constructor() {
        super();
    }

    static async getUser(userId: string): Promise<IResultObject<IAppUserModel>> {
        return await this.get<IAppUserModel>(`AppUser/${userId}`);
    }

    static async getContestUsersToAddToTeam(contestId: string, teamId: string): Promise<IResultObject<IAppUser[]>> {
        return await this.get<IAppUser[]>(`AppUser/users/${contestId}/${teamId}`);
    }

    static async putUser(userId: string, data:IAppUserModel): Promise<IResultObject<IAppUserModel>> {
        return await this.put<IAppUserModel>(`AppUser/${userId}`,data);
    }

}
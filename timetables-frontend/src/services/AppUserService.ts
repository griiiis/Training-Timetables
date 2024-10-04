import BaseService from "./BaseService";
import { IResultObject } from "./IResultObject";
import { IAppUser } from "@/domain/Identity/IAppUser";
import { IAppUserModel } from "@/domain/DTOs/Users/IAppUserModel";


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
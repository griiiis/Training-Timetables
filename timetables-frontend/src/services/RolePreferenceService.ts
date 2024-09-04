import { IRolePreference } from "@/domain/IRolePreference";
import { IResultObject } from "./IResultObject";
import BaseService from "./BaseService";

export default class RolePreferenceService extends BaseService{

    private constructor() {
        super();
    }

    //kasutan
    static async getAll(): Promise<IResultObject<IRolePreference[]>> {
        return await this.get<IRolePreference[]>(`RolePreferences/`);
    }

    static async addRolePreferences(data:object): Promise<IResultObject<IRolePreference[]>> {
        return await this.post<IRolePreference[]>(`RolePreferences/`,data);
    }
}
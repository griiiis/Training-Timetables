import BaseService from "./BaseService";
import { IResultObject } from "./IResultObject";
import { IUserInfo } from "@/state/AppContext";
import { ILogOutInfo } from "@/domain/Identity/ILogOutInfo";

export default class AccountService extends BaseService {
    private constructor() {
        super();
    }
    
    static async login(data: object): Promise<IResultObject<IUserInfo>> {

        return this.tryAndCatch<IUserInfo>(async () => {
            return (await this.httpClient()).post<IUserInfo>(`Identity/Account/Login`, data);
    })}

    static async register(data: object): Promise<IResultObject<IUserInfo>> {
        return this.tryAndCatch<IUserInfo>(async () => {
            return (await this.httpClient()).post<IUserInfo>(`Identity/Account/Register`, data);
    })}

    static async logout(data: ILogOutInfo) : Promise<IResultObject<IUserInfo>> {
        if(!data.refreshToken) return {};

        return this.tryAndCatch<IUserInfo>(async () => {
            return (await this.httpClient()).post<IUserInfo>("Identity/Account/Logout", data)
    })}
}
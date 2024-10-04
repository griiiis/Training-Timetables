import BaseService from "./BaseService";
import { IResultObject } from "./IResultObject";
import { IUserInfo } from "@/domain/Identity/IUserInfo";
import { ILogOutInfo } from "@/domain/Identity/ILogOutInfo";

export default class AccountService extends BaseService {
    private constructor() {
        super();
    }
    
    static async login(data: object): Promise<IResultObject<IUserInfo>> {

        return this.tryAndCatch<IUserInfo>(async () => {
            return (await this.httpClient()).post<IUserInfo>(`identity/Account/Login`, data);
    })}

    static async register(data: object): Promise<IResultObject<IUserInfo>> {
        return this.tryAndCatch<IUserInfo>(async () => {
            return (await this.httpClient()).post<IUserInfo>(`identity/Account/Register`, data);
    })}

    static async logout(data: ILogOutInfo) : Promise<IResultObject<IUserInfo>> {
        if(!data.refreshToken) return {};

        return this.tryAndCatch<IUserInfo>(async () => {
            return (await this.httpClient()).post<IUserInfo>("identity/Account/Logout", data)
    })}
}
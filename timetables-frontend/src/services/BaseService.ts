import { IUserInfo } from "@/state/AppContext";
import axios, { AxiosResponse } from "axios";
import { IResultObject } from "./IResultObject";
import { ITokenRefreshInfo } from "@/domain/Identity/ITokenRefreshInfo";

export default class BaseService {

    protected constructor() {
    }


    protected static async delete<T>(url: string): Promise<IResultObject<T>> {
        return this.tryAndCatch<T>(async () => {
            return (await this.httpClient()).delete<T>(url)
        });
    }

    protected static async put<T>(url: string, data: object): Promise<IResultObject<T>> {
        return this.tryAndCatch<T>(async () => {
            return (await this.httpClient()).put<T>(url, data)
        });
    }

    protected static async post<T>(url: string, data: object): Promise<IResultObject<T>> {
        return this.tryAndCatch<T>(async () => {
            return (await this.httpClient()).post<T>(url, data)
        });
    }

    protected static async get<T>(url: string): Promise<IResultObject<T>> {
        return this.tryAndCatch<T>(async () => {
            return (await this.httpClient()).get<T>(url)
        });
    }

    protected static async tryAndCatch<T>(request: () => Promise<AxiosResponse<T, any>>): Promise<IResultObject<T>> {
        try {
            const response = await request();
            return {
                data: response.data
            }
        }
        catch (error: any) {
            return {
                errors: typeof error.response.data === 'object'
                    ? Object.values(error.response.data.errors).flat()
                    : [error.response.data]
            };
        }
    }

    protected static async httpClient() {

        const json = localStorage.getItem("userInfo");

        if (json === null) {
            return axios.create({
                baseURL: 'http://localhost:5220/api/v1/'
            });
        }

        const userInfo : IUserInfo = JSON.parse(json);
        if (userInfo.expiresIn < Date.now() && userInfo.jwt) {
            const data: ITokenRefreshInfo = {
                jwt: userInfo.jwt,
                refreshToken: userInfo.refreshToken
            }

            const res = await this.tryAndCatch<IUserInfo>(async () => {
                return await axios.create({
                    baseURL: 'http://localhost:5220/api/v1/',
                    headers: {
                        "Authorization": "Bearer " + userInfo.jwt
                    }
            }).post<IUserInfo>(`identity/Account/RefreshTokenData`, data)});
            if (res.data){
                if (res.data) {
                    userInfo.jwt = res.data.jwt;
                    userInfo.refreshToken = res.data.refreshToken;
                    userInfo.expiresIn = Date.now() + res.data.expiresIn * 1000;
                    localStorage.setItem("userInfo", JSON.stringify(userInfo));
                } else {
                    console.log("Not logged in");
                }
            }
        }
        
        return axios.create({
            baseURL: 'http://localhost:5220/api/v1/',
            headers: {
                "Authorization": "Bearer " + userInfo.jwt,
                "Content-Type": "application/json"
            }
        });
    }
}
import { ITime } from "@/domain/ITime";
import { IResultObject } from "./IResultObject";
import BaseService from "./BaseService";


export default class TimeService extends BaseService {

    private constructor() {
        super();
    }

    static async getAll(): Promise<IResultObject<ITime[]>> {
        return await this.get<ITime[]>(`Times`);
    }

    static async getCurrentContestTimes(contestId : string): Promise<IResultObject<ITime[]>> {
        return await this.get<ITime[]>(`Times/${contestId}`);
    }

    static async getTime(TimeId : string): Promise<IResultObject<ITime>> {
        return await this.get<ITime>(`Times/owner/${TimeId}`);
    }

    static async postTime(data: object): Promise<IResultObject<ITime>> {
        return await this.post<ITime>(`Times`, data);
    }

    static async deleteTime(TimeId: string): Promise<IResultObject<ITime>> {
        return await this.delete<ITime>(`Times/${TimeId}`);
    }

    static async putTime(TimeId: string, data: object): Promise<IResultObject<ITime>> {
        return await this.put<ITime>(`Times/${TimeId}`, data);
    }
}
import { ITimeOfDay } from "@/domain/ITimeOfDay";
import { IResultObject } from "./IResultObject";
import BaseService from "./BaseService";


export default class TimeOfDayService extends BaseService {

    private constructor() {
        super();
    }

    static async getAll(): Promise<IResultObject<ITimeOfDay[]>> {
        return await this.get<ITimeOfDay[]>(`TimeOfDays`);
    }

    static async getContestTimeOfDays(contestId: string): Promise<IResultObject<ITimeOfDay[]>> {
        return await this.get<ITimeOfDay[]>(`TimeOfDays/contest/${contestId}`);
    }

    static async getTimeOfDay(TimeOfDayId : string): Promise<IResultObject<ITimeOfDay>> {
        return await this.get<ITimeOfDay>(`TimeOfDays/${TimeOfDayId}`);
    }

    static async postTimeOfDay(data: object): Promise<IResultObject<ITimeOfDay>> {
        return await this.post<ITimeOfDay>(`TimeOfDays`, data);
    }

    static async deleteTimeOfDay(TimeOfDayId: string): Promise<IResultObject<ITimeOfDay>> {
        return await this.delete<ITimeOfDay>(`TimeOfDays/${TimeOfDayId}`);
    }

    static async putTimeOfDay(TimeOfDayId: string, data: object): Promise<IResultObject<ITimeOfDay>> {
        return await this.put<ITimeOfDay>(`TimeOfDays/${TimeOfDayId}`, data);
    }
}
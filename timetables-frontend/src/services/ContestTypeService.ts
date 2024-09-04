import { IResultObject } from "./IResultObject";
import { IContestType } from "@/domain/IContestType";
import BaseService from "./BaseService";

export default class ContestTypeService extends BaseService {

    private constructor() {
        super();
    }

    static async getAll(): Promise<IResultObject<IContestType[]>> {
        return await this.get<IContestType[]>(`ContestTypes`);
    }

    static async getContestType(contestTypeId : string): Promise<IResultObject<IContestType>> {
        return await this.get<IContestType>(`ContestTypes/${contestTypeId}`);
    }

    static async postContestType(data: object): Promise<IResultObject<IContestType>> {
        return await this.post<IContestType>(`ContestTypes`, data);
    }

    static async deleteContestType(contestTypeId: string): Promise<IResultObject<IContestType>> {
        return await this.delete<IContestType>(`ContestTypes/${contestTypeId}`);
    }

    static async putContestType(contestTypeId: string, data: object): Promise<IResultObject<IContestType>> {
        return await this.put<IContestType>(`ContestTypes/${contestTypeId}`, data);
    }
}
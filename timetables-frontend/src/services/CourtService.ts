import { IResultObject } from "./IResultObject";
import { ICourt } from "@/domain/ICourt";
import BaseService from "./BaseService";


export default class CourtService extends BaseService {

    private constructor() {
        super();
    }

    static async getAll(): Promise<IResultObject<ICourt[]>> {
        return await this.get<ICourt[]>(`Courts`);
    }

    static async getCourt(CourtId : string): Promise<IResultObject<ICourt>> {
        return await this.get<ICourt>(`Courts/${CourtId}`);
    }

    static async postCourt(data: object): Promise<IResultObject<ICourt>> {
        return await this.post<ICourt>(`Courts`, data);
    }

    static async deleteCourt(CourtId: string): Promise<IResultObject<ICourt>> {
        return await this.delete<ICourt>(`Courts/${CourtId}`);
    }

    static async putCourt(CourtId: string, data: object): Promise<IResultObject<ICourt>> {
        return await this.put<ICourt>(`Courts/${CourtId}`, data);
    }
}
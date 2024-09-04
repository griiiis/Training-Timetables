import { IGameType } from "@/domain/IGameType";
import { IResultObject } from "./IResultObject";
import BaseService from "./BaseService";


export default class GameTypeService extends BaseService {
    private constructor() {
        super();
    }

    static async getAll(): Promise<IResultObject<IGameType[]>> {
        return await this.get<IGameType[]>(`GameTypes`);
    }

    static async getCurrentContestGameTypes(contestId: string): Promise<IResultObject<IGameType[]>> {
        return await this.get<IGameType[]>(`GameTypes/${contestId}`);
    }
    
    //Kasutan
    static async getGameTypeForAll(GameTypeId : string): Promise<IResultObject<IGameType>> {
        return await this.get<IGameType>(`GameTypes/gameType/${GameTypeId}`);
    }

    //Kasutan
    static async getGameType(GameTypeId : string): Promise<IResultObject<IGameType>> {
        return await this.get<IGameType>(`GameTypes/owner/${GameTypeId}`);
    }

    static async postGameType(data: object): Promise<IResultObject<IGameType>> {
        return await this.post<IGameType>(`GameTypes`, data);
    }

    static async deleteGameType(GameTypeId: string): Promise<IResultObject<IGameType>> {
        return await this.delete<IGameType>(`GameTypes/${GameTypeId}`);
    }

    static async putGameType(GameTypeId: string, data: object): Promise<IResultObject<IGameType>> {
        return await this.put<IGameType>(`GameTypes/${GameTypeId}`, data);
    }
}
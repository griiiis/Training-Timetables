import { ILevel } from "@/domain/ILevel";
import { IResultObject } from "./IResultObject";
import BaseService from "./BaseService";


export default class LevelService extends BaseService {
    private constructor() {
        super();
    }

    // Returns all levels visible to current user
    static async getAll(): Promise<IResultObject<ILevel[]>> {
        return await this.get<ILevel[]>(`Levels`);
    }

    static async getCurrentContestLevels(contestId: string): Promise<IResultObject<ILevel[]>> {
        return await this.get<ILevel[]>(`Levels/contest/${contestId}`);
    }   

    static async getLevel(LevelId : string): Promise<IResultObject<ILevel>> {
        return await this.get<ILevel>(`Levels/${LevelId}`);
    }

    static async postLevel(data: object): Promise<IResultObject<ILevel>> {
        return await this.post<ILevel>(`Levels`, data);
    }

    static async putLevel(LevelId: string, data: object): Promise<IResultObject<ILevel>> {
        return await this.put<ILevel>(`Levels/${LevelId}`, data);
    }

    static async deleteLevel(LevelId: string): Promise<IResultObject<ILevel>> {
        return await this.delete<ILevel>(`Levels/${LevelId}`);
    }
}
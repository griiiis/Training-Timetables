import { IResultObject } from "./IResultObject";
import { IGame } from "@/domain/IGame";
import BaseService from "./BaseService";


export default class GameService extends BaseService {
    private constructor() {
        super();
    }

    //kasutan
    static async getUserContestGames(contestId: string): Promise<IResultObject<IGame[]>> {
        return await this.get<IGame[]>(`Games/userGames/${contestId}`);
    }

    static async getGame(GameId : string): Promise<IResultObject<IGame>> {
        return await this.get<IGame>(`Games/${GameId}`);
    }

    //kasutan
    static async getContestGames(contestId: string): Promise<IResultObject<IGame[]>> {
        return await this.get<IGame[]>(`Games/contestGames/${contestId}`);
    }

    //kasutan
    static async anyContestGames(contestId : string): Promise<IResultObject<boolean>> {
        return await this.get<boolean>(`Games/anyGames/${contestId}`);
    }

    static async postGame(contestId: string, data: object): Promise<IResultObject<IGame>> {
        return await this.post<IGame>(`Games/${contestId}`, data);
    }

    static async deleteGame(GameId: string): Promise<IResultObject<IGame>> {
        return await this.delete<IGame>(`Games/${GameId}`);
    }

    static async putGame(GameId: string, data: object): Promise<IResultObject<IGame>> {
        return await this.put<IGame>(`Games/${GameId}`, data);
    }
}
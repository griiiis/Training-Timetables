import { IUserTeam } from "@/domain/IUserTeam";
import { IResultObject } from "./IResultObject";
import BaseService from "./BaseService";

export default class UserTeamService extends BaseService {
    private constructor() {
        super();
    }

    static async getAllUserTeams(): Promise<IResultObject<IUserTeam[]>> {
        return await this.get<IUserTeam[]>(`UserTeams/user`);
    }

    static async getCurrentContestUserTeams(contestId: string): Promise<IResultObject<IUserTeam[]>> {
        return await this.get<IUserTeam[]>(`UserTeams/contest/${contestId}`);
    }

    static async AddToTeam(contestId: string, userId: string, data: object): Promise<IResultObject<IUserTeam>> {
        return await this.post<IUserTeam>(`UserTeams/AddToTeam/${contestId}`, data);
    }

    static async deleteUserTeam(UserTeamId: string): Promise<IResultObject<IUserTeam>> {
        return await this.delete<IUserTeam>(`UserTeams/${UserTeamId}`);
    }

    static async putUserTeam(UserTeamId: string, data: object): Promise<IResultObject<IUserTeam>> {
        return await this.put<IUserTeam>(`UserTeams/${UserTeamId}`, data);
    }
}
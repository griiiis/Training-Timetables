import { ITeam } from "@/domain/ITeam";
import { IResultObject } from "./IResultObject";
import BaseService from "./BaseService";


export default class TeamService extends BaseService {
    private constructor() {
        super();
    }

    static async getCurrentContestTeams(contestId: string): Promise<IResultObject<ITeam[]>> {
        return await this.get<ITeam[]>(`Teams/${contestId}`);
    }
}
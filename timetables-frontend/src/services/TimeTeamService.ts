import { IUserTeam } from "@/domain/IUserTeam";
import { IResultObject } from "./IResultObject";
import BaseService from "./BaseService";
import { ITimeTeam } from "@/domain/ITimeTeam";

export default class TimeTeamService extends BaseService {
    private constructor() {
        super();
    }

    static async getCurrentTimeTeams(contestId: string): Promise<IResultObject<ITimeTeam[]>> {
        return await this.get<ITimeTeam[]>(`TimeTeams/${contestId}`);
    }
}
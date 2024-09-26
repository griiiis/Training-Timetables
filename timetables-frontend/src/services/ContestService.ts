import { IResultObject } from "./IResultObject";
import { IContest } from "@/domain/IContest";
import BaseService from "./BaseService";
import { IContestEditModel } from "@/domain/Models/Contests/IContestEditModel";

export default class ContestService extends BaseService {
    private constructor() {
        super();
    }

    static async getAll(): Promise<IResultObject<IContest[]>> {
        return await this.get<IContest[]>("Contests");
    }

    static async getUserContests(): Promise<IResultObject<IContest[]>> {
        return await this.get<IContest[]>(`Contests/user`);
    }

    static async getAllOwnerContests(): Promise<IResultObject<IContest[]>> {
        return await this.get<IContest[]>(`Contests/owner`);
    }

    static async getContestInformation(contestId: string): Promise<IResultObject<IContest>> {
        return await this.get<IContest>(`Contests/${contestId}`);
    }

    static async getEditContest(contestId : string): Promise<IResultObject<IContestEditModel>> {
        return await this.get<IContestEditModel>(`Contests/owner/edit/${contestId}`);
    }

    static async getContest(contestId : string): Promise<IResultObject<IContest>> {
        return await this.get<IContest>(`Contests/owner/${contestId}`);
    }

    static async postContest(data: object): Promise<IResultObject<IContest>> {
        return await this.post<IContest>(`Contests`, data);
    }

    static async deleteContest(contestId: string, ): Promise<IResultObject<IContest>> {
        return await this.delete<IContest>(`Contests/owner/${contestId}`);
    }

    static async putContest(contestId: string, data: object): Promise<IResultObject<IContest>> {
        return await this.put<IContest>(`Contests/owner/${contestId}`, data);
    }
}
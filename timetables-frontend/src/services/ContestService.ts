import { IResultObject } from "./IResultObject";
import { IContest } from "@/domain/IContest";
import { IEditContestDTO } from "@/domain/DTOs/Contests/IEditContestDTO";
import { IDeleteContestDTO } from "@/domain/DTOs/Contests/IDeleteContestDTO";
import { IOwnerContestsDTO } from "@/domain/DTOs/Contests/IOwnerContestsDTO";
import { IInformationContestDTO } from "@/domain/DTOs/Contests/IInformationContestDTO";
import BaseService from "./BaseService";
import { IMyContestsDTO } from "@/domain/DTOs/Contests/IMyContestsDTO";
import { ICreateContestDTO } from "@/domain/DTOs/Contests/ICreateContestDTO";

export default class ContestService extends BaseService {
    private constructor() {
        super();
    }

    static async getAll(): Promise<IResultObject<IContest[]>> {
        return await this.get<IContest[]>("Contests");
    }

    static async getUserContests(): Promise<IResultObject<IMyContestsDTO>> {
        return await this.get<IMyContestsDTO>(`Contests/user`);
    }

    static async getContestInformation(contestId: string): Promise<IResultObject<IInformationContestDTO>> {
        return await this.get<IInformationContestDTO>(`Contests/${contestId}`);
    }

    static async getAllOwnerContests(): Promise<IResultObject<IOwnerContestsDTO[]>> {
        return await this.get<IOwnerContestsDTO[]>(`Contests/owner`);
    }

    static async getEditContest(contestId : string): Promise<IResultObject<IEditContestDTO>> {
        return await this.get<IEditContestDTO>(`Contests/owner/edit/${contestId}`);
    }

    static async getOwnerContest(contestId : string): Promise<IResultObject<IDeleteContestDTO>> {
        return await this.get<IDeleteContestDTO>(`Contests/owner/${contestId}`);
    }

    static async postContest(contestDTO: ICreateContestDTO): Promise<IResultObject<IContest>> {
        return await this.post<IContest>(`Contests`, contestDTO);
    }

    static async putContest(contestDTO: IEditContestDTO): Promise<IResultObject<IContest>> {
        return await this.put<IContest>(`Contests/${contestDTO.id}`, contestDTO);
    }

    static async deleteContest(contestId: string): Promise<IResultObject<IContest>> {
        return await this.delete<IContest>(`Contests/${contestId}`);
    }
}
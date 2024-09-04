import { IUserContestPackage } from "@/domain/IUserContestPackage";
import { IResultObject } from "./IResultObject";
import BaseService from "./BaseService";


export default class UserContestPackageService extends BaseService {

    private constructor() {
        super();
    }

    static async getAll(): Promise<IResultObject<IUserContestPackage[]>> {
        return await this.get<IUserContestPackage[]>(`UserContestPackages`);
    }

    static async getCurrentUserPackages(): Promise<IResultObject<IUserContestPackage[]>> {
        return await this.get<IUserContestPackage[]>(`UserContestPackages/user`);
    }

    static async getContestAllUsers(contestId : string): Promise<IResultObject<IUserContestPackage[]>> {
        return await this.get<IUserContestPackage[]>(`UserContestPackages/contestusers/${contestId}`);
    }

    static async getContestUsersWithoutTrainers(contestId: string): Promise<IResultObject<IUserContestPackage[]>> {
        return await this.get<IUserContestPackage[]>(`UserContestPackages/users/${contestId}`);
    }

    static async getContestTeammates(contestId: string, teamId: string): Promise<IResultObject<IUserContestPackage[]>> {
        return await this.get<IUserContestPackage[]>(`UserContestPackages/users/${contestId}/${teamId}`);
    }

    static async AddToTeam(teamId: string, data: object): Promise<IResultObject<IUserContestPackage>> {
        return await this.post<IUserContestPackage>(`UserContestPackages/AddToTeam/${teamId}`, data);
    }

    static async getContestTeachers(contestId: string): Promise<IResultObject<IUserContestPackage[]>> {
        return await this.get<IUserContestPackage[]>(`UserContestPackages/teachers/${contestId}`);
    }

    static async anyTeams(contestId: string): Promise<IResultObject<boolean>> {
        return await this.get<boolean>(`UserContestPackages/teams/${contestId}`);
    }

    static async getUserContestPackage(UserContestPackageId : string): Promise<IResultObject<IUserContestPackage>> {
        return await this.get<IUserContestPackage>(`UserContestPackages/${UserContestPackageId}`);
    }

    static async postUserContestPackage(data: object): Promise<IResultObject<IUserContestPackage>> {
        return await this.post<IUserContestPackage>(`UserContestPackages`, data);
    }

    static async deleteUserContestPackage(UserContestPackageId: string): Promise<IResultObject<IUserContestPackage>> {
        return await this.delete<IUserContestPackage>(`UserContestPackages/${UserContestPackageId}`);
    }

    static async putUserContestPackage(UserContestPackageId: string, data: object): Promise<IResultObject<IUserContestPackage>> {
        return await this.put<IUserContestPackage>(`UserContestPackages/${UserContestPackageId}`, data);
    }
}
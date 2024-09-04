import { IResultObject } from "./IResultObject";
import BaseService from "./BaseService";
import { IPackageGameTypeTime } from "@/domain/IPackageGameTypeTime";


export default class PackageGameTypeTimeService extends BaseService {
    private constructor() {
        super();
    }
    
    static async getAll(): Promise<IResultObject<IPackageGameTypeTime[]>> {
        return await this.get<IPackageGameTypeTime[]>(`PackageGameTypeTimes/owner`);
    }

    static async getCurrentContestPackages(contestId: string): Promise<IResultObject<IPackageGameTypeTime[]>> {
        return await this.get<IPackageGameTypeTime[]>(`PackageGameTypeTimes/${contestId}`);
    }

    static async getPackageGameTypeTime(PackageGameTypeTimeId : string): Promise<IResultObject<IPackageGameTypeTime>> {
        return await this.get<IPackageGameTypeTime>(`PackageGameTypeTimes/owner/${PackageGameTypeTimeId}`);
    }

    static async getPackageGameTypeTimeForAll(PackageGameTypeTimeId : string): Promise<IResultObject<IPackageGameTypeTime>> {
        return await this.get<IPackageGameTypeTime>(`PackageGameTypeTimes/package/${PackageGameTypeTimeId}`);
    }

    static async postPackageGameTypeTime(data: object): Promise<IResultObject<IPackageGameTypeTime>> {
        return await this.post<IPackageGameTypeTime>(`PackageGameTypeTimes`, data);
    }

    static async deletePackageGameTypeTime(PackageGameTypeTimeId: string): Promise<IResultObject<IPackageGameTypeTime>> {
        return await this.delete<IPackageGameTypeTime>(`PackageGameTypeTimes/owner/${PackageGameTypeTimeId}`);
    }

    static async putPackageGameTypeTime(PackageGameTypeTimeId: string, data: object): Promise<IResultObject<IPackageGameTypeTime>> {
        return await this.put<IPackageGameTypeTime>(`PackageGameTypeTimes/owner/${PackageGameTypeTimeId}`, data);
    }
}
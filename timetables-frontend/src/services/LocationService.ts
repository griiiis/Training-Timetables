import { ILocation } from "@/domain/ILocation";
import { IResultObject } from "./IResultObject";
import BaseService from "./BaseService";


export default class LocationService extends BaseService {
    private constructor() {
        super();
    }

    static async getAll(): Promise<IResultObject<ILocation[]>> {
        return await this.get<ILocation[]>(`Locations`);
    }

    static async getLocation(LocationId : string): Promise<IResultObject<ILocation>> {
        return await this.get<ILocation>(`Locations/${LocationId}`);
    }

    static async postLocation(data: object): Promise<IResultObject<ILocation>> {
        return await this.post<ILocation>(`Locations`, data);
    }

    static async deleteLocation(LocationId: string): Promise<IResultObject<ILocation>> {
        return await this.delete<ILocation>(`Locations/${LocationId}`);
    }

    static async putLocation(LocationId: string, data: object): Promise<IResultObject<ILocation>> {
        return await this.put<ILocation>(`Locations/${LocationId}`, data);
    }
}
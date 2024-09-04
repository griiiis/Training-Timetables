"use client"
import { IContestType } from "@/domain/IContestType";
import { ILevel } from "@/domain/ILevel";
import { ILocation } from "@/domain/ILocation";
import { IPackageGameTypeTime } from "@/domain/IPackageGameTypeTime";
import { ITime } from "@/domain/ITime";
import { IContestCreateModel } from "@/domain/Models/Contests/IContestCreateModel";
import ContestService from "@/services/ContestService";
import ContestTypeService from "@/services/ContestTypeService";
import GameTypeService from "@/services/GameTypeService";
import LevelService from "@/services/LevelService";
import LocationService from "@/services/LocationService";
import PackageGameTypeTimeService from "@/services/PackageGameTypeTimeService";
import TimeService from "@/services/TimeService";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Create() {
    const router = useRouter();
    const [contestName, setContestName] = useState("");
    const [description, setDescription] = useState("");
    const [from, setFrom] = useState('');
    const [until, setUntil] = useState('');
    const [totalHours, setTotalHours] = useState(0);
    const [contestTypeId, setContestTypeId] = useState("");
    const [contestTypes, setContestTypes] = useState<IContestType[]>([]);
    const [locations, setLocations] = useState<ILocation[]>([]);
    const [locationId, setLocationId] = useState("");
    const [levels, setLevels] = useState<ILevel[]>([]);
    const [levelIds, setLevelIds] = useState(Array());
    const [packages, setPackages] = useState<IPackageGameTypeTime[]>([]);
    const [packagesIds, setPackagesIds] = useState(Array());
    const [times, setTimes] = useState<ITime[]>([]);
    const [timesIds, setTimesIds] = useState(Array());
    const [validationError, setValidationError] = useState("");
    const [isLoading, setIsLoading] = useState(true);

    const CreateNewContest = async () => {
        const createContestData : IContestCreateModel = {
            contest: {
            contestName: contestName,
            description: description,
            from: from,
            until: until,
            totalHours: totalHours,
            contestTypeId: contestTypeId,
            locationId: locationId,
            },
            selectedLevelIds: levelIds,
            selectedTimesIds: timesIds,
            selectedPackagesIds: packagesIds,
        };
        const response = await ContestService.postContest(createContestData);
        if (response.data) {
            router.push("/ContestAdmin/Contest");
        }
        if (response.errors && response.errors.length > 0) {
            setValidationError(response.errors[0]);
        }
    };

    const loadData = async () => {
        const timesResponse = await TimeService.getAll();
        const packageResponse = await PackageGameTypeTimeService.getAll();
        const gameTypeResponse = await GameTypeService.getAll();
        const levelResponse = await LevelService.getAll();
        const contestTypeResponse = await ContestTypeService.getAll();
        const locationResponse = await LocationService.getAll();
        if (contestTypeResponse.data && locationResponse.data && levelResponse.data && timesResponse.data && packageResponse.data && gameTypeResponse.data) {
            setContestTypes(contestTypeResponse.data);
            setLocations(locationResponse.data);
            setTimes(timesResponse.data);
            setPackages(packageResponse.data);
            setLevels(levelResponse.data)
            setIsLoading(false);
        };
    }
    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Create new contest - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Create New Contest</h1>
            <hr />
            <br />
            <form>
                <div className="text-danger" role="alert">{validationError}</div>
                <div className="mb-3 row">
                    <div className="col-md-4">
                        <div className="form-group">
                            <label className="control-label" htmlFor="ContestName">Contest Name</label>
                            <input className="form-control" type="text" id="ContestName" value={contestName} onChange={(e) => { setContestName(e.target.value); setValidationError(""); }} />
                        </div>
                        <br />
                        <div className="form-group">
                            <label className="control-label" htmlFor="From">From</label>
                            <input className="form-control" type="datetime-local" id="From" value={from.toString().substring(0, 16)} onChange={(e) => { setFrom(e.target.value); setValidationError(""); }} />
                        </div>
                        <br />
                        <div className="form-group">
                            <label className="control-label" htmlFor="Location">Location</label>
                            <select className="form-control" onChange={(e) => { setLocationId(e.target.value); setValidationError(""); }}><option>Please choose one option</option>{locations.map((location) => {
                                return (
                                    <option key={location.id} value={location.id}>
                                        {location.locationName}
                                    </option>
                                );
                            })}</select>
                        </div>
                        <br />
                        <div className="form-group">
                            <label className="control-label" htmlFor="Levels">Levels</label>
                            <select multiple className="form-control" onChange={(e) => { setLevelIds(Array.from(e.target.selectedOptions, option => option.value)); setValidationError(""); }}
                            >{levels.map((level) => {
                                return (
                                    <option key={level.id} value={level.id}>
                                        {level.title}
                                    </option>
                                );
                            })}</select>
                        </div>
                    </div>
                    <div className="col-md-4">
                        <div className="form-group">
                            <label className="control-label" htmlFor="Description">Description</label>
                            <input className="form-control" type="text" id="Description" value={description} onChange={(e) => { setDescription(e.target.value); setValidationError(""); }} />
                        </div>
                        <br />
                        <div className="form-group">
                            <label className="control-label" htmlFor="Until">Until</label>
                            <input className="form-control" type="datetime-local" id="Until" value={until.toString().substring(0, 16)} onChange={(e) => { setUntil(e.target.value); setValidationError(""); }} />
                        </div>
                        <br />
                        <div className="form-group">
                            <label className="control-label" htmlFor="ContestType">Contest Type</label>
                            <select required className="form-control" onChange={(e) => { setContestTypeId(e.target.value); setValidationError(""); }}><option>Please choose one option</option>{contestTypes.map((contestType) => {
                                return (
                                    <option key={contestType.id} value={contestType.id}>
                                        {contestType.contestTypeName}
                                    </option>
                                );
                            })}</select>
                        </div>
                        <br />
                        <div className="form-group">
                            <label className="control-label" htmlFor="Packages">Packages</label>
                            <select required multiple className="form-control" onChange={(e) => { setPackagesIds(Array.from(e.target.selectedOptions, option => option.value)); setValidationError(""); }}>{packages.map((apackage) => {
                                return (
                                    <option key={apackage.id} value={apackage.id}>
                                        {apackage.packageGtName}
                                    </option>
                                );
                            })}</select>
                        </div>
                    </div>
                    <br />
                    <div className="col-md-4">
                        <div className="form-group">
                            <label className="control-label" htmlFor="TotalHours">TotalHours</label>
                            <input required className="form-control" type="number" id="TotalHours" value={totalHours} onChange={(e) => { setTotalHours(Number.parseInt(e.target.value)); setValidationError(""); }} />
                        </div>
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <div className="form-group">
                            <label className="control-label" htmlFor="Time of Days">Times</label>
                            <select multiple className="form-control" onChange={(e) => { setTimesIds(Array.from(e.target.selectedOptions, option => option.value)); setValidationError(""); }}>{times.map((time) => {
                                return (
                                    <option key={time.id} value={time.id}>
                                        {`${time.from} - ${time.until}`}
                                    </option>
                                );
                            })}</select>
                        </div>
                    </div>
                    
                    <div className="form-group middle">
                    <br/>
                    <br/>
                    <br/>
                        <button onClick={(e) => { CreateNewContest(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                    </div>
                </div>
            </form>
            <div>
                <Link className="middle" href="/ContestAdmin/Contest">Back to List</Link>
            </div>
        </>
    );
}
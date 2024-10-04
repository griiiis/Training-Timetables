"use client"
import { IEditContestDTO } from "@/domain/DTOs/Contests/IEditContestDTO";
import { IContestType } from "@/domain/IContestType";
import { ILevel } from "@/domain/ILevel"
import { ILocation } from "@/domain/ILocation";
import { IPackageGameTypeTime } from "@/domain/IPackageGameTypeTime";
import { ITime } from "@/domain/ITime";
import ContestService from "@/services/ContestService";
import ContestTypeService from "@/services/ContestTypeService";
import LevelService from "@/services/LevelService";
import LocationService from "@/services/LocationService";
import PackageGameTypeTimeService from "@/services/PackageGameTypeTimeService";
import TimeService from "@/services/TimeService";
import Link from "next/link"
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Edit() {
    let { id } = useParams();
    const router = useRouter();
    const [contestName, setContestName] = useState("");
    const [description, setDescription] = useState("");
    const [from, setFrom] = useState("");
    const [until, setUntil] = useState("");
    const [totalHours, setTotalHours] = useState("0");
    const [contestTypeId, setContestTypeId] = useState("");
    const [locationId, setLocationId] = useState("");

    const [previousLevelIds, setPreviousLevelIds] = useState(Array());
    const [previousPackagesIds, setPreviousPackagesIds] = useState(Array());
    const [previousTimesIds, setPreviousTimesIds] = useState(Array());


    const [contestTypes, setContestTypes] = useState<IContestType[]>([]);
    const [locations, setLocations] = useState<ILocation[]>([]);
    const [levels, setLevels] = useState<ILevel[]>([]);
    const [packages, setPackages] = useState<IPackageGameTypeTime[]>([]);
    const [times, setTimes] = useState<ITime[]>([]);

    const [validationError, setValidationError] = useState("");
    const [isLoading, setIsLoading] = useState(true);

    const loadData = async () => {
        const responseContest = await ContestService.getEditContest(id.toString());
        const responseLevels = await LevelService.getAll();
        const responseTimes = await TimeService.getAll();
        const responsePackages = await PackageGameTypeTimeService.getAll();
        const responseLocations = await LocationService.getAll();
        const responseContestTypes = await ContestTypeService.getAll();

        if (responseContest.data && responseLevels.data && responseTimes.data && responsePackages.data && responseLocations.data && responseContestTypes.data) {
            // Previous data about the contest
            setContestName(responseContest.data.contestName);
            setDescription(responseContest.data.description)
            setFrom(responseContest.data.from.toString())
            setUntil(responseContest.data.until.toString())
            setTotalHours(responseContest.data.totalHours.toString())
            setLocationId(responseContest.data.locationId)
            setContestTypeId(responseContest.data.contestTypeId)

            setPreviousLevelIds(responseContest.data.levelIds);
            setPreviousPackagesIds(responseContest.data.packagesIds);
            setPreviousTimesIds(responseContest.data.timesIds);


            // All other data (Levels, Times, Packages, Locations, ContestTypes)
            setContestTypes(responseContestTypes.data);
            setLocations(responseLocations.data)
            setPackages(responsePackages.data)
            setLevels(responseLevels.data)
            setTimes(responseTimes.data)
            setIsLoading(false);
        }
    };

    const editContest = async () => {
        const editContestDTO : IEditContestDTO = {
            id: id.toString(),
            contestName: contestName,
            description: description,
            from: from,
            until: until,
            totalHours: Number.parseInt(totalHours),
            contestTypeId: contestTypeId,
            locationId: locationId,
            levelIds: previousLevelIds,
            timesIds: previousTimesIds,
            packagesIds: previousPackagesIds,
        
        }
        console.log(editContestDTO)

        const response = await ContestService.putContest(editContestDTO);
        if (response.data){
            router.push("/ContestAdmin/Contest");
        
        if (response.errors && response.errors.length > 0) {
            setValidationError(response.errors[0]);
        }
    }
}

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Edit contest - LOADING</h1>)
    return (
        <>
            <h1 className="middle">Edit Contest</h1>
            <hr />
            <br />
            <form>
                <div className="text-danger" role="alert">{validationError}</div>
                <div className="mb-3 row">
                    <div className="col-md-4">
                        <div className="form-group">
                            <label className="control-label" htmlFor="ContestName">Contest Name</label>
                            <input className="form-control" type="text" id="ontestName" value={contestName} onChange={(e) => { setContestName(e.target.value); setValidationError(""); }} />
                        </div>
                        <br />
                        <div className="form-group">
                            <label className="control-label" htmlFor="From">From</label>
                            <input className="form-control" type="datetime-local" id="From" value={from.toString().substring(0, 16)} onChange={(e) => { setFrom(e.target.value); setValidationError(""); }} />
                        </div>
                        <br />
                        <div className="form-group">
                            <label className="control-label" htmlFor="Location">Location</label>
                            <select className="form-control" onChange={(e) => { setLocationId(e.target.value); setValidationError(""); }}>
                                {locations.map((location) => {
                                return (
                                    <option key={location.id} value={location.id} selected={locationId === location.id}>
                                        {location.locationName}
                                    </option>
                                );
                            })}</select>
                        </div>
                        <br />
                        <div className="form-group">
                            <label className="control-label" htmlFor="Levels">Levels</label>
                            {previousLevelIds ? (
                                <>
                                    <select multiple className="form-control" required onChange={(e) => { setPreviousLevelIds(Array.from(e.target.selectedOptions, option => option.value)); setValidationError(""); }}>
                                        
                                        {levels.map((level) => {
                                            return (
                                            <option key={level.id} value={level.id} selected={previousLevelIds.some(e => e === level.id)}>
                                                {level.title}
                                            </option>
                                        );})}
                                    </select>
                                </>
                            ) : (
                                <>
                                    <select multiple className="form-control" onChange={(e) => { setPreviousLevelIds(Array.from(e.target.selectedOptions, option => option.value)); setValidationError(""); }}
                                    >{levels.map((level) => {
                                        return (
                                            <option key={level.id} value={level.id}>
                                                {level.title}
                                            </option>
                                        );
                                    })}</select>
                                </>
                            )}
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
                            <select required className="form-control" onChange={(e) => { setContestTypeId(e.target.value); setValidationError(""); }}>
                                {contestTypes.map((contestType) => {
                                return (
                                    <option key={contestType.id} value={contestType.id} selected={contestTypeId === contestType.id}>
                                        {contestType.contestTypeName}
                                    </option>
                                );
                            })}</select>
                        </div>
                        <br />
                        <div className="form-group">
                            <label className="control-label" htmlFor="Packages">Packages</label>
                            {previousPackagesIds ? (
                                <>
                                    <select multiple className="form-control" required onChange={(e) => { setPreviousPackagesIds(Array.from(e.target.selectedOptions, option => option.value)); setValidationError(""); }}>
                                        
                                        {packages.map((packagee) => (
                                            <option key={packagee.id} value={packagee.id} selected={previousPackagesIds.some(e => e === packagee.id)}>
                                                {packagee.packageGtName}
                                            </option>
                                        ))}
                                    </select>
                                </>
                            ) : (
                                <>
                                    <select required multiple className="form-control" onChange={(e) => { setPreviousPackagesIds(Array.from(e.target.selectedOptions, option => option.value)); setValidationError(""); }}>
                                        {packages.map((packagee) => {
                                return (
                                    <option key={packagee.id} value={packagee.id}>
                                        {packagee.packageGtName}
                                    </option>
                                );
                            })}</select>
                                </>
                            )}
                        </div>
                    </div>
                    <br />
                    <div className="col-md-4">
                        <div className="form-group">
                            <label className="control-label" htmlFor="TotalHours">TotalHours</label>
                            <input required className="form-control" type="number" id="TotalHours" value={totalHours} onChange={(e) => { setTotalHours(e.target.value.toString()); setValidationError(""); }} />
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
                            <label className="control-label" htmlFor="Times">Times</label>
                            {previousTimesIds ? (
                                <>
                                    <select multiple className="form-control" required onChange={(e) => { setPreviousTimesIds(Array.from(e.target.selectedOptions, option => option.value)); setValidationError(""); }}>
                                        
                                        {times.map((time) => (
                                            <option key={time.id} value={time.id} selected={previousTimesIds.some(e => e === time.id)}>
                                                {`${time.from} - ${time.until}`}
                                            </option>
                                        ))}
                                    </select>
                                </>
                            ) : (
                                <>
                                    <select multiple className="form-control" onChange={(e) => { setPreviousTimesIds(Array.from(e.target.selectedOptions, option => option.value)); setValidationError(""); }}>
                                        {times.map((time) => {
                                return (
                                    <option key={time.id} value={time.id}>
                                        {`${time.from} - ${time.until}`}
                                    </option>
                                );
                            })}</select>
                                </>
                            )}
                        </div>
                    </div>
                    <div className="form-group middle">
                        <br />
                        <br />
                        <br />
                        <button onClick={(e) => { editContest(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                    </div>
                </div>
            </form>
            <div>
                <Link className="middle" href="/ContestAdmin/Contest">Back to List</Link>
            </div>
        </>
    );
}
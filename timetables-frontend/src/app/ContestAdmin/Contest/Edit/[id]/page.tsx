"use client"
import { IContest } from "@/domain/IContest";
import { IContestType } from "@/domain/IContestType";
import { ILevel } from "@/domain/ILevel"
import { ILocation } from "@/domain/ILocation";
import { IPackageGameTypeTime } from "@/domain/IPackageGameTypeTime";
import { ITime } from "@/domain/ITime";
import ContestService from "@/services/ContestService";
import Link from "next/link"
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Edit() {
    let { id } = useParams();
    const router = useRouter();
    const [contestName, setContestName] = useState("");
    const [description, setDescription] = useState("");
    const [from, setFrom] = useState('');
    const [until, setUntil] = useState('');
    const [totalHours, setTotalHours] = useState("0");
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

    const [previousLevels, setPreviousLevels] = useState<ILevel[]>([]);
    const [previousTimes, setPreviousTimes] = useState<ITime[]>([]);
    const [previousPackages, setPreviousPackages] = useState<IPackageGameTypeTime[]>([]);

    const [validationError, setValidationError] = useState("");
    const [isLoading, setIsLoading] = useState(true);

    const loadData = async () => {
        const response = await ContestService.editContest(id.toString());
        if (response.data) {
            setContestName(response.data.contest.contestName);
            setDescription(response.data.contest.description)
            setFrom(response.data.contest.from.toString())
            setUntil(response.data.contest.until.toString())
            setTotalHours(response.data.contest.totalHours.toString())
            setLocationId(response.data.contest.location.id)
            setContestTypeId(response.data.contest.contestType.id)
            setContestTypes(response.data.contestTypeList);
            setLocations(response.data.locationList)
            setPackages(response.data.packagesList)
            setLevels(response.data.levelList)
            setTimes(response.data.timesList)

            setLevelIds(response.data.previousLevels.map(e => e.id));
            setPackagesIds(response.data.previousPackages.map(e => e.id));
            setTimesIds(response.data.previousTimes.map(e => e.id));

            setPreviousLevels(response.data.previousLevels);
            setPreviousPackages(response.data.previousPackages);
            setPreviousTimes(response.data.previousTimes);
            setIsLoading(false);
        }
    };

    const editContest = async () => {
        const contest :IContest = {
            contestName: contestName,
            id: id.toString(),
            description: description,
            from: from,
            until: until,
            totalHours: Number.parseInt(totalHours),
            contestTypeId: undefined,
            location: undefined,
            contestGameTypes: []
        }
        const contestData = {
            id : id,
            contestName: contestName,
            descrition: description,
            from: from,
            until: until,
            totalHours: totalHours,
            contestTypeId: contestTypeId,
            locatinId: locationId,
            selectedLevelIds: levelIds,
            selectedTimesIds: timesIds,
            selectedPackagesIds: packagesIds,
        };
        const response = await ContestService.putContest(id.toString(), contestData);
        console.log(contestData)
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
                            {previousLevels ? (
                                <>
                                    <select multiple className="form-control" required onChange={(e) => { setLevelIds(Array.from(e.target.selectedOptions, option => option.value)); setValidationError(""); }}>
                                        
                                        {levels.map((level) => (
                                            <option key={level.id} value={level.id} selected={previousLevels.some(e => e.id === level.id)}>
                                                {level.title}
                                            </option>
                                        ))}
                                    </select>
                                </>
                            ) : (
                                <>
                                    <select multiple className="form-control" onChange={(e) => { setLevelIds(Array.from(e.target.selectedOptions, option => option.value)); setValidationError(""); }}
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
                            {previousPackages ? (
                                <>
                                    <select multiple className="form-control" required onChange={(e) => { setPackagesIds(Array.from(e.target.selectedOptions, option => option.value)); setValidationError(""); }}>
                                        
                                        {packages.map((packagee) => (
                                            <option key={packagee.id} value={packagee.id} selected={previousPackages.some(e => e.id === packagee.id)}>
                                                {packagee.packageGtName}
                                            </option>
                                        ))}
                                    </select>
                                </>
                            ) : (
                                <>
                                    <select required multiple className="form-control" onChange={(e) => { setPackagesIds(Array.from(e.target.selectedOptions, option => option.value)); setValidationError(""); }}>
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
                            {previousLevels ? (
                                <>
                                    <select multiple className="form-control" required onChange={(e) => { setTimesIds(Array.from(e.target.selectedOptions, option => option.value)); setValidationError(""); }}>
                                        
                                        {times.map((time) => (
                                            <option key={time.id} value={time.id} selected={previousTimes.some(e => e.id === time.id)}>
                                                {`${time.from} - ${time.until}`}
                                            </option>
                                        ))}
                                    </select>
                                </>
                            ) : (
                                <>
                                    <select multiple className="form-control" onChange={(e) => { setTimesIds(Array.from(e.target.selectedOptions, option => option.value)); setValidationError(""); }}>
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
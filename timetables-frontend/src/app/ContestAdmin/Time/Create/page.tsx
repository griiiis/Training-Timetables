"use client"
import TimeService from "@/services/TimeService";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { ITimeOfDay } from "@/domain/ITimeOfDay";
import TimeOfDayService from "@/services/TimeOfDayService";

export default function Create() {
    const router = useRouter();
    const [from, setFrom] = useState("");
    const [until, setUntil] = useState("");
    const [timeOfDayId, setTimeOfDayId] = useState("");
    const [validationError, setValidationError] = useState("");
    const [timeOfDays, setTimeOfDays] = useState<ITimeOfDay[]>([]);
    const [isLoading, setIsLoading] = useState(true);

    const CreateNewTime = async () => {
        const TimeData = {
            from: from,
            until: until,
            timeOfDayId: timeOfDayId
        };
        const response = await TimeService.postTime(TimeData);
        if (response.data) {
            router.push("/ContestAdmin/Time");
        }
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    };

    useEffect(() => { loadData() }, []);
    const loadData = async () => {
        const timeOfDayResponse = await TimeOfDayService.getAll();
        if (timeOfDayResponse.data) {
            setTimeOfDays(timeOfDayResponse.data);
            setIsLoading(false);
        };
    }

    if (isLoading) return (<h1>Create New Time - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Create New Time</h1>
            <hr />
            <br />
            <form>
            <div className="text-danger" role="alert">{validationError}</div>
                <div className="mb-3 row">
                    <div className="col-md-4">
                        <div className="form-group">
                            <label className="control-label" htmlFor="From">From</label>
                            <input className="form-control" type="time" id="From" value={from} onChange={(e) => { setFrom(e.target.value += ":00"); setValidationError("");}} />
                        </div>
                        <div className="form-group">
                            <label className="control-label" htmlFor="Until">Until</label>
                            <input className="form-control" type="time" id="until" value={until} onChange={(e) => { setUntil(e.target.value += ":00"); setValidationError("");}}/>
                        </div>
                        <div className="form-group">
                            <label className="control-label" htmlFor="Time of Day">Time of Day</label>
                            <select className="form-control" onChange={(e) => {setTimeOfDayId(e.target.value); setValidationError("");}}>
                                <option value="">Please choose one option</option>
                                {timeOfDays.map((item) => {
                                    return (
                                        <option key={item.id} value={item.id}>
                                            {item.timeOfDayName}
                                        </option>
                                    );
                                })}</select>
                        </div>
                        <div className="form-group">
                            <button onClick={(e) => { CreateNewTime(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                        </div>
                    </div>
                </div>
            </form>

            <div>
                <Link href="/ContestAdmin/Time">Back to List</Link>
            </div>
        </>
    );
}
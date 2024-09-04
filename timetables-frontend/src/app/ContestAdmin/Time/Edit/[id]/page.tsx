"use client"
import TimeService from "@/services/TimeService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useContext, useEffect, useState } from "react";
import { ITimeOfDay } from "@/domain/ITimeOfDay";
import TimeOfDayService from "@/services/TimeOfDayService";


export default function Edit() {
    let { id } = useParams();
    const router = useRouter();
    const [from, setFrom] = useState("");
    const [until, setUntil] = useState("");
    const [timeOfDayId, setTimeOfDayId] = useState("");
    const [validationError, setValidationError] = useState("");
    const [timeOfDays, setTimeOfDays] = useState<ITimeOfDay[]>([]);
    const [isLoading, setIsLoading] = useState(true);

    const loadData = async () => {
        const response = await TimeService.getTime(id.toString());
        const timeOfDayResponse = await TimeOfDayService.getAll();
        if (response.data && timeOfDayResponse.data) {
            setFrom(response.data.from.toString());
            setUntil(response.data.until.toString());
            setTimeOfDayId(response.data.timeOfDayId);
            setTimeOfDays(timeOfDayResponse.data);
            setIsLoading(false);
        };
    }

    const editTime = async () => {
        const TimeData = {
            from: from,
            until: until,
            timeOfDayId: timeOfDayId,
            id: id,
        };
        const response = await TimeService.putTime(id.toString(), TimeData);
        if (response.data) {
            router.push("/ContestAdmin/Time");
        }
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    }

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Edit Time - LOADING</h1>)

    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Edit Time</h1>
                    <hr />
                    <br />
                    <form>
                    <div className="text-danger" role="alert">{validationError}</div>
                        <div className="mb-3 row">
                            <div className="col-md-4">
                                <div className="form-group">
                                    <label className="control-label" htmlFor="From">From</label>
                                    <input className="form-control" type="time" id="From" value={from} onChange={(e) => { setFrom(e.target.value); setValidationError(""); }} />
                                </div>
                                <div className="form-group">
                                    <label className="control-label" htmlFor="Until">Until</label>
                                    <input className="form-control" type="time" id="until" value={until} onChange={(e) => { setUntil(e.target.value); setValidationError(""); }} />
                                </div>
                                <div className="form-group">
                                    <label className="control-label" htmlFor="Game Type">Game Type</label>
                                    <select className="form-control" onChange={(e) => {setTimeOfDayId(e.target.value); setValidationError("");}}>
                                        <option value={timeOfDays.find((e) => e.id === timeOfDayId)?.id}>{timeOfDays.find((e) => e.id === timeOfDayId)?.timeOfDayName}</option>
                                        {timeOfDays.map((item) => (item.id !== timeOfDayId && (
                                            <option key={item.id} value={item.id}>
                                                {item.timeOfDayName}
                                            </option>
                                        )))};
                                    </select>
                                </div>

                                <div className="form-group">
                                    <button onClick={(e) => { editTime(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                                </div>

                            </div>
                        </div>
                    </form>

                    <div>
                        <Link href="/ContestAdmin/Time">Back to List</Link>
                    </div>


                </main>
            </div>
        </>
    );
}
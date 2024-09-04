"use client"
import TimeOfDayService from "@/services/TimeOfDayService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Edit() {
    let { id } = useParams();
    const router = useRouter();
    const [TimeOfDayName, setTimeOfDayName] = useState("");
    const [validationError, setValidationError] = useState("");
    const [isLoading, setIsLoading] = useState(true);

    const loadData = async () => {
        const response = await TimeOfDayService.getTimeOfDay(id.toString());

        if (response.data) {
            setTimeOfDayName(response.data.timeOfDayName);
            setIsLoading(false);
        };
    }

    const editTimeOfDay = async () => {
        const TimeOfDayData = {
            TimeOfDayName: TimeOfDayName,
            id: id
        };
        const response = await TimeOfDayService.putTimeOfDay(id.toString(), TimeOfDayData);
        if (response.data) {
            router.push("/ContestAdmin/TimeOfDay");
        }
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    }

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Edit Time Of Day - LOADING</h1>)

    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Edit Time Of Day</h1>
                    <hr />
                    <br />
                    <form>
                    <div className="text-danger" role="alert">{validationError}</div>
                        <div className="mb-3 row">
                            <div className="col-md-4">
                                <div className="form-group">
                                    <label className="control-label" htmlFor="Time Of Day Name">Time Of Day Name</label>
                                    <input className="form-control" type="text" id="TimeOfDayName" value={TimeOfDayName} onChange={(e) => { setTimeOfDayName(e.target.value); setValidationError("");}} />
                                </div>
                                <div className="form-group">
                                    <button onClick={(e) => { editTimeOfDay(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                                </div>
                        </div>
                    </div>
                    </form>
                    <div>
                        <Link href="/ContestAdmin/TimeOfDay">Back to List</Link>
                    </div>


                </main>
            </div>
        </>
    );
}
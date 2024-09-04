"use client"
import TimeOfDayService from "@/services/TimeOfDayService";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useState } from "react";

export default function Create() {
    const router = useRouter();
    const [TimeOfDayName, setTimeOfDayName] = useState("");
    const [validationError, setValidationError] = useState("");

    const CreateNewTimeOfDay = async () => {
        const TimeOfDayData = {
            TimeOfDayName: TimeOfDayName
        };
        const response = await TimeOfDayService.postTimeOfDay(TimeOfDayData);
        if (response.data) {
            router.push("/ContestAdmin/TimeOfDay");
        }
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    };

    return (
        <>
            <h1 className="middle">Create New Time Of Day</h1>
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
                            <button onClick={(e) => { CreateNewTimeOfDay(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                        </div>
                    </div>
                </div>
            </form>
            <div>
                <Link href="/ContestAdmin/TimeOfDay">Back to List</Link>
            </div>
        </>
    );
}
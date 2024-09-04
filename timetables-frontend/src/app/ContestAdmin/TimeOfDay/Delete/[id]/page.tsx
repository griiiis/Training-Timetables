"use client"
import { ITimeOfDay } from "@/domain/ITimeOfDay";
import TimeOfDayService from "@/services/TimeOfDayService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";


export default function Delete() {
    let { id } = useParams();
    const router = useRouter();
    const [TimeOfDay, setTimeOfDay] = useState<ITimeOfDay>();
    const [isLoading, setIsLoading] = useState(true);

    const loadData = async () => {
        const response = await TimeOfDayService.getTimeOfDay(id.toString());
        if (response.data) {
            setTimeOfDay(response.data);
            setIsLoading(false);
        }
    };

    const deleteTimeOfDay = async () => {
        await TimeOfDayService.deleteTimeOfDay(id.toString());
        router.push("/ContestAdmin/TimeOfDay");
    }

    useEffect(() => { loadData() }, [])

    if (isLoading) return (<h1>Delete Time Of Day - LOADING</h1>)
    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Delete Time Of Day</h1>
                    <div>
                        <hr />
                        <br />
                        <dl className="row">
                            <div className="col-md-6">
                                <div className="card">
                                    <div className="card-body">
                                        <dt className="col-sm-4">
                                            Name
                                        </dt>
                                        <dd className="col-sm-8">
                                            {TimeOfDay?.timeOfDayName}
                                        </dd>
                                    </div>
                                </div>
                            </div>
                        </dl>
                        <form>
                            <button onClick={(e) => { deleteTimeOfDay(), e.preventDefault(); }} type="submit" className="btn btn-danger">Delete</button> |
                            <Link href="/ContestAdmin/TimeOfDay">Back to List</Link>
                        </form>
                    </div>

                </main>
            </div>
        </>
    );
}
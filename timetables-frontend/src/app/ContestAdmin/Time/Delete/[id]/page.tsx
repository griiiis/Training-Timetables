"use client"
import { ITime } from "@/domain/ITime";
import TimeService from "@/services/TimeService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";


export default function Delete() {
    let { id } = useParams();
    const router = useRouter();
    const [Time, setTime] = useState<ITime>();
    const [isLoading, setIsLoading] = useState(true);
    const loadData = async () => {
        const response = await TimeService.getTime(id.toString());
        if (response.data) {
            setTime(response.data);
            setIsLoading(false);
        }
    };

    const deleteTime = async () => {
        await TimeService.deleteTime(id.toString());
        router.push("/ContestAdmin/Time");
    }

    useEffect(() => { loadData() }, [])

    if (isLoading) return (<h1>Delete Time - LOADING</h1>)
    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Delete Time</h1>
                    <div>
                         <hr />
                        <br />
                        <dl className="row">
                            <div className="col-md-6">
                                <div className="card">
                                    <div className="card-body">
                            <dt className="col-sm-4">
                                From
                            </dt>
                            <dd className="col-sm-8">
                               {Time?.from}
                            </dd>
                            <dt className="col-sm-4">
                                Until
                            </dt>
                            <dd className="col-sm-8">
                               {Time?.until}
                            </dd>
                            <dt className="col-sm-4">
                                Time Of Day
                            </dt>
                            <dd className="col-sm-8">
                               {Time?.timeOfDay.timeOfDayName}
                            </dd>
                            </div>
                                </div>
                            </div>
                        </dl>
                        <form>
                        <button onClick={(e) => { deleteTime(), e.preventDefault(); }} type="submit" className="btn btn-danger">Delete</button> |
                            <Link href="/ContestAdmin/Time">Back to List</Link>
                            </form>
                    </div>

                </main>
            </div>
        </>
    );
}
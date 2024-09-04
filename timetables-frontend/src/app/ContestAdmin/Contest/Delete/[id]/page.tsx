"use client"
import { IContest } from "@/domain/IContest";
import ContestService from "@/services/ContestService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";


export default function Delete() {
    let {id} = useParams();
    const router = useRouter();
    const [contest, setContest] = useState<IContest>();
    const [isLoading, setIsLoading] = useState(true);

    const getContestDetails = async () => {
        const response = await ContestService.getContest(id.toString());
        if (response.data) {
            setContest(response.data);
            setIsLoading(false);
        }
    };

    const deleteContest = async () => {
        await ContestService.deleteContest(id.toString());
        router.push("/ContestAdmin/Contest");
    }

    useEffect(() => { getContestDetails() }, [])
    if (isLoading) return (<h1>Delete contest - LOADING</h1>)

    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Delete Contest</h1>
                    <div>
                        <hr />
                        <br />
                        <dl className="row">
                            <div className="col-md-6">
                                <div className="card">
                                    <div className="card-body">
                                        <dt className="col-sm-4">
                                            Contest Name
                                        </dt>
                                        <dd className="col-sm-8">
                                            {contest?.contestName}
                                        </dd>
                                        <dt className="col-sm-4">
                                            Description
                                        </dt>
                                        <dd className="col-sm-8">
                                            {contest?.description}
                                        </dd>
                                        <dt className="col-sm-4">
                                            From
                                        </dt>
                                        <dd className="col-sm-8">
                                            {contest?.from}
                                        </dd>
                                        <dt className="col-sm-4">
                                            Until
                                        </dt>
                                        <dd className="col-sm-8">
                                            {contest?.until}
                                        </dd>
                                    </div>
                                </div>
                            </div>
                            <div className="col-md-6">
                                <div className="card">
                                    <div className="card-body">
                                        <dt className="col-sm-4">
                                            Total Hours
                                        </dt>
                                        <dd className="col-sm-8">
                                            {contest?.totalHours}
                                        </dd>
                                        <dt className="col-sm-4">
                                            Contest Type
                                        </dt>
                                        <dd className="col-sm-8">
                                            {contest?.contestType.contestTypeName}
                                        </dd>
                                        <dt className="col-sm-4">
                                            Location
                                        </dt>
                                        <dd className="col-sm-8">
                                            {contest?.location.locationName}
                                        </dd>
                                    </div>
                                </div>
                            </div>
                        </dl>
                        <form>
                            <button onClick={(e) => { deleteContest(), e.preventDefault(); }} type="submit" className="btn btn-danger">Delete</button> |
                            <Link href="/ContestAdmin/Contest">Back to List</Link>
                        </form>
                    </div>
                </main >
            </div >
        </>
    );
}
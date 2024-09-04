"use client"
import { ICourt } from "@/domain/ICourt";
import CourtService from "@/services/CourtService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useContext, useEffect, useState } from "react";

export default function Delete() {
    let { id } = useParams();
    const router = useRouter();
    const [court, setCourt] = useState<ICourt>();
    const [isLoading, setIsLoading] = useState(true);

    const loadData = async () => {
        const response = await CourtService.getCourt(id.toString());
        if (response.data) {
            setCourt(response.data);
            setIsLoading(false);
        }
    };

    const deleteCourt = async () => {
        await CourtService.deleteCourt(id.toString());
        router.push("/ContestAdmin/Court");
    }

    useEffect(() => { loadData() }, [])

    if (isLoading) return (<h1>Delete Court - LOADING</h1>)

    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Delete Court</h1>
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
                                            {court?.courtName}
                                        </dd>
                                        <dt className="col-sm-4">
                                            Game Type
                                        </dt>
                                        <dd className="col-sm-8">
                                            {court?.gameType.gameTypeName}
                                        </dd>
                                        <dt className="col-sm-4">
                                            Location
                                        </dt>
                                        <dd className="col-sm-8">
                                            {court?.location.locationName}
                                        </dd>
                                    </div>
                                </div>
                            </div>
                        </dl>
                        <form>
                            <button onClick={(e) => { deleteCourt(), e.preventDefault(); }} type="submit" className="btn btn-danger">Delete</button> |
                            <Link href="/ContestAdmin/Court">Back to List</Link>
                        </form>
                    </div>

                </main>
            </div>
        </>
    );
}
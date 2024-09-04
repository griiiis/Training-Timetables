"use client"
import { ILocation } from "@/domain/ILocation";
import LocationService from "@/services/LocationService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";


export default function Delete() {
    let { id } = useParams();
    const router = useRouter();
    const [location, setLocation] = useState<ILocation>();
    const [isLoading, setIsLoading] = useState(true);

    const loadData = async () => {
        const response = await LocationService.getLocation(id.toString());

        if (response.data) {
            setLocation(response.data);
            setIsLoading(false);
        }
    };

    const deleteLocation = async () => {
        await LocationService.deleteLocation(id.toString());
        router.push("/ContestAdmin/Location");
    }

    useEffect(() => { loadData() }, [])

    if (isLoading) return (<h1>Delete location - LOADING</h1>)
    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Delete Location</h1>
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
                                            {location?.locationName}
                                        </dd>
                                        <dt className="col-sm-4">
                                            State
                                        </dt>
                                        <dd className="col-sm-8">
                                            {location?.state}
                                        </dd>
                                        <dt className="col-sm-4">
                                            Country
                                        </dt>
                                        <dd className="col-sm-8">
                                            {location?.country}
                                        </dd>
                                    </div>
                                </div>
                            </div>
                        </dl>
                        <form>
                            <button onClick={(e) => { deleteLocation(), e.preventDefault(); }} type="submit" className="btn btn-danger">Delete</button> |
                            <Link href="/ContestAdmin/Location">Back to List</Link>
                        </form>
                    </div>

                </main>
            </div>
        </>
    );
}
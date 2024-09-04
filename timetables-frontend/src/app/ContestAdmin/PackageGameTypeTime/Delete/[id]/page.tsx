"use client"
import { IPackageGameTypeTime } from "@/domain/IPackageGameTypeTime";
import PackageGameTypeTimeService from "@/services/PackageGameTypeTimeService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";


export default function Delete() {
    let { id } = useParams();
    const router = useRouter();
    const [PackageGameTypeTime, setPackageGameTypeTime] = useState<IPackageGameTypeTime>();
    const [isLoading, setIsLoading] = useState(true);
    const loadData = async () => {
        const response = await PackageGameTypeTimeService.getPackageGameTypeTime(id.toString());

        if (response.data) {
            setPackageGameTypeTime(response.data);
            setIsLoading(false);
        }
    };

    const deletePackageGameTypeTime = async () => {
        await PackageGameTypeTimeService.deletePackageGameTypeTime(id.toString());
        router.push("/ContestAdmin/PackageGameTypeTime");
    }

    useEffect(() => { loadData() }, [])

    if (isLoading) return (<h1>Delete Package - LOADING</h1>)
    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Delete Package</h1>
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
                                            {PackageGameTypeTime?.packageGtName}
                                        </dd>
                                        <dt className="col-sm-4">
                                            Game Type
                                        </dt>
                                        <dd className="col-sm-8">
                                            {PackageGameTypeTime?.gameType.gameTypeName}
                                        </dd>
                                        <dt className="col-sm-4">
                                            Part of the whole
                                        </dt>
                                        <dd className="col-sm-8">
                                            {PackageGameTypeTime?.times}
                                        </dd>
                                    </div>
                                </div>
                            </div>
                        </dl>
                        <form>
                            <button onClick={(e) => { deletePackageGameTypeTime(), e.preventDefault(); }} type="submit" className="btn btn-danger">Delete</button> |
                            <Link href="/ContestAdmin/PackageGameTypeTime">Back to List</Link>
                        </form>
                    </div>

                </main>
            </div>
        </>
    );
}
"use client"
import { IContestType } from "@/domain/IContestType";
import ContestTypeService from "@/services/ContestTypeService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useContext, useEffect, useState } from "react";


export default function Delete() {
    let { id } = useParams();
    const router = useRouter();
    const [contestType, setContestType] = useState<IContestType>();
    const [isLoading, setIsLoading] = useState(true);

    const loadData = async () => {
        const response = await ContestTypeService.getContestType(id.toString());
        if (response.data) {
            setContestType(response.data);
            setIsLoading(false);
        }
    };

    const deleteContestType = async () => {
        await ContestTypeService.deleteContestType(id.toString());
        router.push("/ContestAdmin/ContestType");
    }

    useEffect(() => { loadData() }, [])

    if (isLoading) return (<h1>Delete Contest Type - LOADING</h1>)

    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Delete Contest Type</h1>
                    <div>
                        <hr />
                        <br/>
                        <dl className="row">
                            <div className="col-md-6">
                                <div className="card">
                                    <div className="card-body">
                            <dt className="col-sm-4">
                                Name
                            </dt>
                            <dd className="col-sm-8">
                               {contestType?.contestTypeName}
                            </dd>
                            <dt className="col-sm-4">
                                Description
                            </dt>
                            <dd className="col-sm-8">
                            {contestType?.description}
                            </dd>
                            </div>
                                </div>
                            </div>
                        </dl>
                        <form>
                        <button onClick={(e) => { deleteContestType(), e.preventDefault(); }} type="submit" className="btn btn-danger">Delete</button> |
                            <Link href="/ContestAdmin/ContestType">Back to List</Link>
                            </form>
                    </div>
                </main>
            </div>
        </>
    );
}
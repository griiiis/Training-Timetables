"use client"
import { ILevel } from "@/domain/ILevel";
import LevelService from "@/services/LevelService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Delete() {
    let { id } = useParams();
    const router = useRouter();
    const [Level, setLevel] = useState<ILevel>();
    const [isLoading, setIsLoading] = useState(true);

    const loadData = async () => {
        const response = await LevelService.getLevel(id.toString());
        if (response.data) {
            setLevel(response.data);
            setIsLoading(false);
        }
    };

    const deleteLevel = async () => {
        await LevelService.deleteLevel(id.toString());
        router.push("/ContestAdmin/Level");
    }

    useEffect(() => { loadData() }, [])

    if (isLoading) return (<h1>Delete Level - LOADING</h1>)
    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Delete Level</h1>
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
                                            {Level?.title}
                                        </dd>
                                        <dt className="col-sm-4">
                                            Description
                                        </dt>
                                        <dd className="col-sm-8">
                                            {Level?.description}
                                        </dd>
                                        
                                    </div>
                                </div>
                            </div>
                        </dl>
                        <form>
                            <button onClick={(e) => { deleteLevel(), e.preventDefault(); }} type="submit" className="btn btn-danger">Delete</button> |
                            <Link href="/ContestAdmin/Level">Back to List</Link>
                        </form>
                    </div>

                </main>
            </div>
        </>
    );
}
"use client"
import { IGameType } from "@/domain/IGameType";
import GameTypeService from "@/services/GameTypeService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Delete() {
    let { id } = useParams();
    const router = useRouter();
    const [gameType, setGameType] = useState<IGameType>();
    const [isLoading, setIsLoading] = useState(true);

    const loadData = async () => {
        const response = await GameTypeService.getGameType(id.toString());
        if (response.data) {
            setGameType(response.data);
            setIsLoading(false);
        }
    };

    const deleteGameType = async () => {
        await GameTypeService.deleteGameType(id.toString());
        router.push("/ContestAdmin/GameType");
    }

    useEffect(() => { loadData() }, [])

    if (isLoading) return (<h1>Delete Game Type - LOADING</h1>)
    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Delete Game Type</h1>
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
                                            {gameType?.gameTypeName}
                                        </dd>
                                    </div>
                                </div>
                            </div>
                        </dl>
                        <form>
                            <button onClick={(e) => { deleteGameType(), e.preventDefault(); }} type="submit" className="btn btn-danger">Delete</button> |
                            <Link href="/ContestAdmin/GameType">Back to List</Link>
                        </form>
                    </div>
                </main>
            </div>
        </>
    );
}
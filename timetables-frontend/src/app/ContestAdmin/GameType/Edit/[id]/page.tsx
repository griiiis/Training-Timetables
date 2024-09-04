"use client"
import GameTypeService from "@/services/GameTypeService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Edit() {
    let { id } = useParams();
    const router = useRouter();
    const [gameTypeName, setGameTypeName] = useState("");
    const [isLoading, setIsLoading] = useState(true);

    const [validationError, setValidationError] = useState("");

    const loadData = async () => {
        const response = await GameTypeService.getGameType(id.toString());
        if (response.data) {
            setGameTypeName(response.data.gameTypeName);
            setIsLoading(false);
        };
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    }

    const editGameType = async () => {
        const gameData = {
            gameTypeName: gameTypeName,
            id: id,
        };
        const response = await GameTypeService.putGameType(id.toString(), gameData);
        if (response.data) {
            router.push("/ContestAdmin/GameType");
        }
    }

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Edit Game Type - LOADING</h1>)

    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Edit Game Type</h1>
                    <hr />
                    <br />
                    <form>
                    <div className="text-danger" role="alert">{validationError}</div>
                        <div className="mb-3 row">
                            <div className="col-md-4">
                                <div className="form-group">
                                    <label className="control-label" htmlFor="Game Type Name">Game Type Name</label>
                                    <input className="form-control" type="text" id="gameTypeName" value={gameTypeName} onChange={(e) => { setGameTypeName(e.target.value); setValidationError("");}} />
                                </div>
                                <div className="form-group">
                                    <button onClick={(e) => { editGameType(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                                </div>
                            </div>
                        </div>
                    </form>
                    <div>
                        <Link href="/ContestAdmin/GameType">Back to List</Link>
                    </div>
                </main>
            </div>
        </>
    );
}
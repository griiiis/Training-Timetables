"use client"
import GameTypeService from "@/services/GameTypeService";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useState } from "react";

export default function Create() {
    const router = useRouter();
    const [gameTypeName, setGameTypeName] = useState("");
    const [validationError, setValidationError] = useState("");

    const CreateNewGameType = async () => {
        const gameData = {
            gameTypeName: gameTypeName,
        };
        const response = await GameTypeService.postGameType(gameData);
        if (response.data) {
            router.push("/ContestAdmin/GameType");
        }
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    };

    return (
        <>
            <h1 className="middle">Create New Game Type</h1>
            <hr />
            <br />
            <form>
            <div className="text-danger" role="alert">{validationError}</div>
                <div className="mb-3 row">
                    <div className="col-md-4">
                        <div className="form-group">
                            <label className="control-label" htmlFor="Game Type">Game Type Name</label>
                            <input className="form-control" type="text" id="gameTypeName" value={gameTypeName} onChange={(e) => { setGameTypeName(e.target.value); setValidationError("");}} />
                        </div>
                        <div className="form-group">
                            <button onClick={(e) => { CreateNewGameType(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                        </div>
                    </div>
                </div>
            </form>
            <div>
                <Link href="/ContestAdmin/GameType">Back to List</Link>
            </div>
        </>
    );
}
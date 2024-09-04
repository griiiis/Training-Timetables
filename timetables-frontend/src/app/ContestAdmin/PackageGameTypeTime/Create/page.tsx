"use client"
import PackageGameTypeTimeService from "@/services/PackageGameTypeTimeService";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { IGameType } from "@/domain/IGameType";
import GameTypeService from "@/services/GameTypeService";

export default function Create() {
    const router = useRouter();
    const [packageGTName, setPackageGTName] = useState("");
    const [gameTypeId, setGameTypeId] = useState("");
    const [times, setTimes] = useState("0");
    const [gameTypes, setGameTypes] = useState<IGameType[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [validationError, setValidationError] = useState("");

    const CreateNewPackageGameTypeTime = async () => {
        const PackageGameTypeTimeData = {
            packageGTName: packageGTName,
            gameTypeId: gameTypeId,
            times: times
        };
        const response = await PackageGameTypeTimeService.postPackageGameTypeTime(PackageGameTypeTimeData);
        if (response.data) {
            router.push("/ContestAdmin/PackageGameTypeTime");
        }
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    };

    useEffect(() => { loadData() }, []);
    const loadData = async () => {
        const gameTypesResponse = await GameTypeService.getAll();
        if (gameTypesResponse.data) {
            setGameTypes(gameTypesResponse.data);
            setIsLoading(false);
        };
    }

    if (isLoading) return (<h1>Create New Package - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Create New Package</h1>
            <hr />
            <br />
            <form>
            <div className="text-danger" role="alert">{validationError}</div>
                <div className="mb-3 row">
                    <div className="col-md-4">
                        <div className="form-group">
                            <label className="control-label" htmlFor="Package name">Package name</label>
                            <input className="form-control" type="text" id="packageGTName" value={packageGTName} onChange={(e) => { setPackageGTName(e.target.value); setValidationError("");}} />
                        </div>
                        <div className="form-group">
                            <label className="control-label" htmlFor="Times">Times</label>
                            <input className="form-control" type="number" id="times" value={times} onChange={(e) => { setTimes(e.target.value); setValidationError("");}} />
                        </div>

                        <div className="form-group">
                            <label className="control-label" htmlFor="Game Type">Game Type</label>
                            <select className="form-control" onChange={(e) => setGameTypeId(e.target.value)}>
                                <option value="">Please choose one option</option>
                                {gameTypes.map((item) => {
                                    return (
                                        <option key={item.id} value={item.id}>
                                            {item.gameTypeName}
                                        </option>
                                    );
                                })}</select>
                        </div>
                        <div className="form-group">
                            <button onClick={(e) => { CreateNewPackageGameTypeTime(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                        </div>

                    </div>
                </div>
            </form>
            <div>
                <Link href="/ContestAdmin/PackageGameTypeTime">Back to List</Link>
            </div>
        </>
    );
}
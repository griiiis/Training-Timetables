"use client"
import PackageGameTypeTimeService from "@/services/PackageGameTypeTimeService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { IGameType } from "@/domain/IGameType";
import GameTypeService from "@/services/GameTypeService";


export default function Edit() {
    let { id } = useParams();
    const router = useRouter();
    const [packageGTName, setPackageGTName] = useState("");
    const [gameTypeId, setGameTypeId] = useState("");
    const [times, setTimes] = useState("0");
    const [gameTypes, setGameTypes] = useState<IGameType[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [validationError, setValidationError] = useState("");

    const loadData = async () => {
        const response = await PackageGameTypeTimeService.getPackageGameTypeTime(id.toString());

        const gameTypesResponse = await GameTypeService.getAll();

        if (response.data && gameTypesResponse.data) {
            setPackageGTName(response.data.packageGtName);
            setGameTypeId(response.data.gameTypeId);
            setTimes(response.data.times.toString());
            setGameTypes(gameTypesResponse.data);
            setIsLoading(false);
        };
    }

    const editPackageGameTypeTime = async () => {
        const PackageGameTypeTimeData = {
            packageGTName: packageGTName,
            gameTypeId: gameTypeId,
            times: times,
            id: id,
        };
        const response = await PackageGameTypeTimeService.putPackageGameTypeTime(id.toString(), PackageGameTypeTimeData);
        if (response.data) {
            router.push("/ContestAdmin/PackageGameTypeTime");
        }
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    }

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Edit Package - LOADING</h1>)

    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Edit Package</h1>
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
                                    <select className="form-control" onChange={(e) => {setGameTypeId(e.target.value); setValidationError("");}}>
                                        <option value={gameTypes.find((e) => e.id === gameTypeId)?.id}>{gameTypes.find((e) => e.id === gameTypeId)?.gameTypeName}</option>
                                        {gameTypes.map((item) => (item.id !== gameTypeId &&
                                            (
                                                <option key={item.id} value={item.id}>
                                                    {item.gameTypeName}
                                                </option>
                                            )))};

                                    </select>
                                </div>
                                <div className="form-group">
                                    <button onClick={(e) => { editPackageGameTypeTime(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                                </div>
                            </div>
                        </div>
                    </form>

                    <div>
                        <Link href="/ContestAdmin/PackageGameTypeTime">Back to List</Link>
                    </div>


                </main>
            </div>
        </>
    );
}
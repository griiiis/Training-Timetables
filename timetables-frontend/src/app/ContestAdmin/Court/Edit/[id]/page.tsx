"use client"
import { IGameType } from "@/domain/IGameType";
import { ILocation } from "@/domain/ILocation";
import CourtService from "@/services/CourtService";
import GameTypeService from "@/services/GameTypeService";
import LocationService from "@/services/LocationService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useContext, useEffect, useState } from "react";

export default function Edit() {
    let { id } = useParams();
    const router = useRouter();
    const [courtName, setCourtName] = useState("");
    const [gameTypeId, setGameTypeId] = useState("");
    const [locationId, setLocationId] = useState("");
    const [gameTypes, setGameTypes] = useState<IGameType[]>([]);
    const [locations, setLocations] = useState<ILocation[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [validationError, setValidationError] = useState("");

    const loadData = async () => {
        const response = await CourtService.getCourt(id.toString());
        const gameTypeResponse = await GameTypeService.getAll();
        const locationResponse = await LocationService.getAll();

        if (response.data && gameTypeResponse.data && locationResponse.data) {
            setCourtName(response.data.courtName);
            setGameTypeId(response.data.gameTypeId)
            setLocationId(response.data.locationId)
            setLocations(locationResponse.data)
            setGameTypes(gameTypeResponse.data)
            setIsLoading(false);
        }
    };

    const editCourt = async () => {
        const CourtData = {
            courtName: courtName,
            gameTypeId: gameTypeId,
            locationId: locationId,
            id : id
        }
        const response = await CourtService.putCourt(id.toString(), CourtData);
        if (response.data) {
            router.push("/ContestAdmin/Court");
        }
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    }

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Edit Court - LOADING</h1>)

    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Edit Court</h1>
                    <hr />
                    <br/>
                    <div className="text-danger" role="alert">{validationError}</div>
                    <div className="row">
                        <div className="col-md-4">
                            <form>
                                <div className="form-group">
                                    <label className="control-label" htmlFor="Name">Name</label>
                                    <input className="form-control" type="text" id="Name" value={courtName} onChange={(e) => { setCourtName(e.target.value); setValidationError("");}} />
                                </div>
                                <div className="form-group">
                                    <label className="control-label" htmlFor="GameType">Game Type</label>
                                    <select className="form-control" onChange={(e) => {setGameTypeId(e.target.value); setValidationError("");}}>
                                    <option value={gameTypes.find((e) => e.id === gameTypeId)?.id}>{gameTypes.find((e) => e.id === gameTypeId)?.gameTypeName}</option>
                                    {gameTypes.map((gameType) => (gameType.id !== gameTypeId && (
                                            <option key={gameType.id} value={gameType.id}>
                                                {gameType.gameTypeName}
                                            </option>
                                        )))}
                                    </select>
                                </div>
                                <div className="form-group">
                                    <label className="control-label" htmlFor="Location">Location</label>
                                    <select className="form-control" onChange={(e) => {setLocationId(e.target.value); setValidationError("");}}>
                                    <option value={locations.find((e) => e.id === locationId)?.id}>{locations.find((e) => e.id === locationId)?.locationName}</option>
                                    
                                    {locations.map((location) => (location.id !== locationId && (
                                            <option key={location.id} value={location.id}>
                                                {location.locationName}
                                            </option>
                                        )))}
                                    </select>
                                </div>
                                <div className="form-group">
                                    <button onClick={(e) => { editCourt(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                                </div>
                            </form>
                        </div>
                    </div>
                    <div>
                        <Link href="/ContestAdmin/Court">Back to List</Link>
                    </div>
                </main>
            </div>
        </>
    );
}
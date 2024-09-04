"use client"
import { IGameType } from "@/domain/IGameType";
import { ILocation } from "@/domain/ILocation";
import CourtService from "@/services/CourtService";
import GameTypeService from "@/services/GameTypeService";
import LocationService from "@/services/LocationService";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useContext, useEffect, useState } from "react";

export default function Create() {
    const router = useRouter();
    const [courtName, setCourtName] = useState("");
    const [gameTypeId, setGameTypeId] = useState("");
    const [locationId, setLocationId] = useState("");
    const [gameTypes, setGameTypes] = useState<IGameType[]>([]);
    const [locations, setLocations] = useState<ILocation[]>([]);
    const [isLoading, setIsLoading] = useState(true);

    const [validationError, setValidationError] = useState("");

    const CreateNewCourt = async () => {
        const CourtData = {
            courtName: courtName,
            gameTypeId: gameTypeId,
            locationId: locationId
        };
        const response = await CourtService.postCourt(CourtData);
        if (response.data) {
            router.push("/ContestAdmin/Court");
        }
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    };

    useEffect(() => { loadData() }, []);
    const loadData = async () => {
        const gameTypeResponse = await GameTypeService.getAll();
        const locationResponse = await LocationService.getAll();
        if (gameTypeResponse.data && locationResponse.data) {
            setGameTypes(gameTypeResponse.data);
            setLocations(locationResponse.data);
            setIsLoading(false);
        };
    }

    if (isLoading) return (<h1>Create New Court - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Create New Court</h1>
            <hr />
            <br />
            <form>
            <div className="text-danger" role="alert">{validationError}</div>
                <div className="mb-3 row">
                    <div className="col-md-4">
                        <div className="form-group">
                            <label className="control-label" htmlFor="Name">Name</label>
                            <input className="form-control" type="text" id="Name" value={courtName} onChange={(e) => { setCourtName(e.target.value); setValidationError("");}} />
                        </div>
                        <div className="form-group">
                            <label className="control-label" htmlFor="GameType">Game Type</label>
                            <select className="form-control" onChange={(e) => {setGameTypeId(e.target.value); setValidationError("");}}><option>Please choose one option</option>{gameTypes.map((gameType) => {
                                return (
                                    <option key={gameType.id} value={gameType.id}>
                                        {gameType.gameTypeName}
                                    </option>
                                );
                            })}</select>
                        </div>
                        <div className="form-group">
                            <label className="control-label" htmlFor="Location">Location</label>
                            <select className="form-control" onChange={(e) => {setLocationId(e.target.value); setValidationError("");}}><option>Please choose one option</option>{locations.map((location) => {
                                return (
                                    <option key={location.id} value={location.id}>
                                        {location.locationName}
                                    </option>
                                );
                            })}</select>
                        </div>
                        <div className="form-group">
                            <button onClick={(e) => { CreateNewCourt(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                        </div>
                    </div>
                </div>
            </form>
            <div>
                <Link href="/ContestAdmin/Court">Back to List</Link>
            </div>
        </>
    );
}
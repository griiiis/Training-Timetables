"use client"
import LocationService from "@/services/LocationService";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useState } from "react";

export default function Create() {
    const router = useRouter();
    const [locationName, setLocationName] = useState("");
    const [state, setState] = useState("");
    const [country, setCountry] = useState("");
    const [validationError, setValidationError] = useState("");

    const CreateNewLocation = async () => {
        const LocationData = {
            locationName: locationName,
            state: state,
            country: country,
        };
        const response = await LocationService.postLocation(LocationData);
        if (response.data) {
            router.push("/ContestAdmin/Location");
        }
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    };

    return (
        <>
            <h1 className="middle">Create New Location</h1>
            <hr />
            <br />
            <form>
            <div className="text-danger" role="alert">{validationError}</div>
                <div className="mb-3 row">
                    <div className="col-md-4">
                        <div className="form-group">
                            <label className="control-label" htmlFor="Location Name">Location Name</label>
                            <input className="form-control" type="text" id="locationName" value={locationName} onChange={(e) => { setLocationName(e.target.value); setValidationError("");}} />
                        </div>
                        <div className="form-group">
                            <label className="control-label" htmlFor="State">State</label>
                            <input className="form-control" type="text" id="state" value={state} onChange={(e) => { setState(e.target.value); setValidationError("");}} />
                        </div>
                        <div className="form-group">
                            <label className="control-label" htmlFor="Country">Country</label>
                            <input className="form-control" type="text" id="country" value={country} onChange={(e) => { setCountry(e.target.value); setValidationError("");}} />
                        </div>
                        <div className="form-group">
                            <button onClick={(e) => { CreateNewLocation(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                        </div>
                    </div>
                </div>
            </form>
            <div>
                <Link href="/ContestAdmin/Location">Back to List</Link>
            </div>
        </>
    );
}
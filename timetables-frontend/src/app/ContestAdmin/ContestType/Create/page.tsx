"use client"
import ContestTypeService from "@/services/ContestTypeService";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useState } from "react";

export default function Create() {
    const router = useRouter();
    const [contestTypeName, setContestTypeName] = useState("");
    const [description, setDescription] = useState("");
    const [validationError, setValidationError] = useState("");

    const CreateNewContestType = async () => {
        const contestTypeData = {
            contestTypeName: contestTypeName,
            description: description
        };
        const response = await ContestTypeService.postContestType(contestTypeData);
        if (response.data){
            router.push("/ContestAdmin/ContestType");
        }
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    };

    return (
        <>
            <h1 className="middle">Create new Contest Type</h1>
            <hr />
            <br />
            <form>
            <div className="text-danger" role="alert">{validationError}</div>
            <div className="mb-3 row">
                <div className="col-md-4">
                        <div className="form-group">
                            <label className="control-label" htmlFor="Name">Name</label>
                            <input className="form-control" type="text" id="Name" value={contestTypeName} onChange={(e) => { setContestTypeName(e.target.value); setValidationError(""); }} />
                        </div>
                        <div className="form-group">
                            <label className="control-label" htmlFor="Description">Description</label>
                            <input className="form-control" type="text" id="Description" value={description} onChange={(e) => { setDescription(e.target.value); setValidationError("");}} />
                        </div>
                        <div className="form-group">
                            <button onClick={(e) => { CreateNewContestType(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                        </div>
                </div>
            </div>
            </form>
            <div>
                <Link href="/ContestAdmin/ContestType">Back to List</Link>
            </div>
        </>
    );
}
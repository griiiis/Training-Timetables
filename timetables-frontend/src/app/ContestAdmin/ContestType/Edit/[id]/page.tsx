"use client"
import ContestTypeService from "@/services/ContestTypeService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Edit() {
    let { id } = useParams();
    const router = useRouter();
    const [contestTypeName, setContestTypeName] = useState("");
    const [description, setDescription] = useState("");
    const [isLoading, setIsLoading] = useState(true);
    const [validationError, setValidationError] = useState("");

    const loadData = async () => {
        const response = await ContestTypeService.getContestType(id.toString());
        if (response.data) {
            setContestTypeName(response.data.contestTypeName)
            setDescription(response.data.description)
            setIsLoading(false);
        }
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    };

    const editContestType = async () => {
        const contestTypeData = {
            contestTypeName: contestTypeName,
            description: description,
            id: id
        }
        const response = await ContestTypeService.putContestType(id.toString(), contestTypeData);
        if (response.data) {
            router.push("/ContestAdmin/ContestType");
        }
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    }

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Edit Contest Type - LOADING</h1>)

    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Edit Contest Type</h1>
                    <hr />
                    <br />
                    <form>
                    <div className="text-danger" role="alert">{validationError}</div>
                        <div className="row">
                            <div className="col-md-4">
                                <div className="form-group">
                                    <label className="control-label" htmlFor="Name">Name</label>
                                    <input className="form-control" type="text" id="Name" value={contestTypeName} onChange={(e) => { setContestTypeName(e.target.value); setValidationError("");}} />
                                </div>
                                <div className="form-group">
                                    <label className="control-label" htmlFor="Description">Description</label>
                                    <input className="form-control" type="text" id="Description" value={description} onChange={(e) => { setDescription(e.target.value); setValidationError("");}} />
                                </div>
                                <div className="form-group">
                                    <button onClick={(e) => { editContestType(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                                </div>
                            </div>
                        </div>
                    </form>
                    <div>
                        <Link href="/ContestAdmin/ContestType">Back to List</Link>
                    </div>
                </main>
            </div>
        </>
    );
}
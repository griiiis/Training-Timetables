"use client"
import LevelService from "@/services/LevelService";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useContext, useState } from "react";

export default function Create() {
    const router = useRouter();
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [validationError, setValidationError] = useState("");

    const CreateNewLevel = async () => {
        const LevelData = {
            title: title,
            description: description,
        };
        const response = await LevelService.postLevel(LevelData);
        if (response.data) {
            router.push("/ContestAdmin/Level");
        }
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    };

    return (
        <>
            <h1 className="middle">Create New Level</h1>
            <hr />
            <br />
            <form>
            <div className="text-danger" role="alert">{validationError}</div>
                <div className="mb-3 row">
                    <div className="col-md-4">
                        <div className="form-group">
                            <label className="control-label" htmlFor="Title">Title</label>
                            <input className="form-control" type="text" id="title" value={title} onChange={(e) => { setTitle(e.target.value); setValidationError("");}} />
                        </div>
                        <div className="form-group">
                            <label className="control-label" htmlFor="Description">Description</label>
                            <input className="form-control" type="text" id="description" value={description} onChange={(e) => { setDescription(e.target.value); setValidationError("");}} />
                        </div>
                        <div className="form-group">
                            <button onClick={(e) => { CreateNewLevel(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                        </div>
                    </div>
                </div>
            </form>
            <div>
                <Link href="/ContestAdmin/Level">Back to List</Link>
            </div>
        </>
    );
}
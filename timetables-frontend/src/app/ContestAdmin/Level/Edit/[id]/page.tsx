"use client"
import LevelService from "@/services/LevelService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Edit() {
    let { id } = useParams();
    const router = useRouter();
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [isLoading, setIsLoading] = useState(true);
    const [validationError, setValidationError] = useState("");

    const loadData = async () => {
        const response = await LevelService.getLevel(id.toString());
        if (response.data) {
            setTitle(response.data.title);
            setDescription(response.data.description);
            setIsLoading(false);
        };
    }

    const editLevel = async () => {
        const LevelData = {
            title: title,
            description: description,
            id: id,
        };
        const response = await LevelService.putLevel(id.toString(), LevelData);
        if (response.data) {
            router.push("/ContestAdmin/Level");
        }
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    }

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Edit Level - LOADING</h1>)

    return (
        <>
            <div className="container">
                <main role="main" className="pb-3">
                    <h1 className="middle">Edit Level</h1>
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
                                    <button onClick={(e) => { editLevel(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                                </div>
                            </div>
                        </div>
                    </form>
                    <div>
                        <Link href="/ContestAdmin/Level">Back to List</Link>
                    </div>
                </main>
            </div>
        </>
    );
}
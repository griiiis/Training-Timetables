"use client"
import { ILevel } from "@/domain/ILevel";
import LevelService from "@/services/LevelService";
import Link from "next/link";
import React, { useContext, useEffect, useState } from "react";

export default function Level() {
    const [isLoading, setIsLoading] = useState(true);
    const [Levels, setLevels] = useState<ILevel[]>([]);
    const [validationError, setValidationError] = useState("");

    const loadData = async () => {
        const response = await LevelService.getAll();
        if (response.data) {
            setLevels(response.data);
            setIsLoading(false);
        }
    }

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Levels - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Levels</h1>
            <p>
                <Link className="display-6 text-dark" href="/ContestAdmin/Level/Create">Create New</Link>
            </p>
            <table className="table table-striped table-hover">
                <thead className="thead-dark">
                    <tr>
                        <th>
                            Level Name
                        </th>
                        <th>
                            Description
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {Levels.map((item) =>
                        <tr key={item.id}>
                            <td>
                                {item.title}
                            </td>
                            <td>
                                {item.description}
                            </td>
                            <td>
                                <Link className="btn btn-primary" href={`/ContestAdmin/Level/Edit/${item.id}`}>Edit</Link> 
                                <Link className="btn btn-danger" href={`/ContestAdmin/Level/Delete/${item.id}`}>Delete</Link>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </>
    );
}

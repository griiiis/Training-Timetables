"use client"
import { ICourt } from "@/domain/ICourt";
import CourtService from "@/services/CourtService";
import Link from "next/link";
import React, { useEffect, useState } from "react";

export default function Court() {
    const [isLoading, setIsLoading] = useState(true);
    const [courts, setCourts] = useState<ICourt[]>([]);

    const loadData = async () => {
        const response = await CourtService.getAll();
        if (response.data) {
            setCourts(response.data);
            setIsLoading(false);
        }
    }
    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Courts - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Courts</h1>
            <p>
                <Link className="display-6 text-dark" href="/ContestAdmin/Court/Create">Create New</Link>
            </p>
            <table className="table table-striped table-hover">
                <thead className="thead-dark">
                    <tr>
                        <th>
                            Court Name
                        </th>
                        <th>
                            Game Type
                        </th>
                        <th>
                            Location
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {courts.map((item) =>
                        <tr key={item.id}>
                            <td>
                                {item.courtName}
                            </td>
                            <td>
                                {item.gameType.gameTypeName}
                            </td>
                            <td>
                                {item.location.locationName}
                            </td>
                            <td>
                                <Link className="btn btn-primary" href={`/ContestAdmin/Court/Edit/${item.id}`}>Edit</Link>
                                <Link className="btn btn-danger"href={`/ContestAdmin/Court/Delete/${item.id}`}>Delete</Link>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </>
    );
}

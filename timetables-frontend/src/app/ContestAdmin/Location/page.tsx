"use client"
import { ILocation } from "@/domain/ILocation";
import LocationService from "@/services/LocationService";
import Link from "next/link";
import React, { useEffect, useState } from "react";

export default function Location() {
    const [isLoading, setIsLoading] = useState(true);
    const [locations, setLocations] = useState<ILocation[]>([]);
    const loadData = async () => {
        const response = await LocationService.getAll();
        if (response.data) {
            setLocations(response.data);
            setIsLoading(false);
        }
    }

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Locations - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Locations</h1>
            <p>
                <Link className="display-6 text-dark" href="/ContestAdmin/Location/Create">Create New</Link>
            </p>
            <table className="table table-striped table-hover">
                <thead className="thead-dark">
                    <tr>
                        <th>
                            Location Name
                        </th>
                        <th>
                            State
                        </th>
                        <th>
                            Country
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {locations.map((item) =>
                        <tr key={item.id}>
                            <td>
                                {item.locationName}
                            </td>
                            <td>
                                {item.state}
                            </td>
                            <td>
                                {item.country}
                            </td>
                            <td>
                                <Link className="btn btn-primary" href={`/ContestAdmin/Location/Edit/${item.id}`}>Edit</Link> 
                                <Link className="btn btn-danger" href={`/ContestAdmin/Location/Delete/${item.id}`}>Delete</Link>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </>
    );
}

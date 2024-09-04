"use client"
import { ITimeOfDay } from "@/domain/ITimeOfDay";
import TimeOfDayService from "@/services/TimeOfDayService";
import Link from "next/link";
import React, { useEffect, useState } from "react";


export default function TimeOfDay() {
    const [isLoading, setIsLoading] = useState(true);
    const [TimeOfDays, setTimeOfDays] = useState<ITimeOfDay[]>([]);

    const loadData = async () => {
        const response = await TimeOfDayService.getAll();
        if (response.data) {
            setTimeOfDays(response.data);
            setIsLoading(false);
        }

    }
    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Time Of Days - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Time Of Days</h1>
            <p>
                <Link className="display-6 text-dark" href="/ContestAdmin/TimeOfDay/Create">Create New</Link>
            </p>
            <table className="table table-striped table-hover">
                <thead className="thead-dark">
                    <tr>
                        <th>
                            Name
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {TimeOfDays.map((item) =>
                        <tr key={item.id}>
                            <td>
                                {item.timeOfDayName}
                            </td>
                            <td>
                                <Link className="btn btn-primary" href={`/ContestAdmin/TimeOfDay/Edit/${item.id}`}>Edit</Link> 
                                <Link className="btn btn-danger" href={`/ContestAdmin/TimeOfDay/Delete/${item.id}`}>Delete</Link>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </>
    );
}

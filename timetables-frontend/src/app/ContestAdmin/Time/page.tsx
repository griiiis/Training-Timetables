"use client"
import { ITime } from "@/domain/ITime";
import TimeService from "@/services/TimeService";
import Link from "next/link";
import React, { useEffect, useState } from "react";

export default function Time() {
    const [isLoading, setIsLoading] = useState(true);
    const [Times, setTimes] = useState<ITime[]>([]);

    const loadData = async () => {
        const response = await TimeService.getAll();

        if (response.data) {
            setTimes(response.data);
            setIsLoading(false);
        }

    }
    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Times - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Times</h1>
            <p>
                <Link className="display-6 text-dark" href="/ContestAdmin/Time/Create">Create New</Link>
            </p>
            <table className="table table-striped table-hover">
                <thead className="thead-dark">
                    <tr>
                        <th>
                            From
                        </th>
                        <th>
                            Until
                        </th>
                        <th>
                            Time Of Day 
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {Times.map((item) =>
                        <tr key={item.id}>
                            <td>
                                {item.from.substring(0,5)}
                            </td>
                            <td>
                                {item.until.substring(0,5)}
                            </td>
                            <td>
                                {item.timeOfDay.timeOfDayName}
                            </td>
                            <td>
                                <Link className="btn btn-primary" href={`/ContestAdmin/Time/Edit/${item.id}`}>Edit</Link> 
                                <Link className="btn btn-danger" href={`/ContestAdmin/Time/Delete/${item.id}`}>Delete</Link>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </>
    );
}

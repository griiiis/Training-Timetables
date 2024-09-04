"use client"
import { IPackageGameTypeTime } from "@/domain/IPackageGameTypeTime";
import PackageGameTypeTimeService from "@/services/PackageGameTypeTimeService";
import Link from "next/link";
import React, { useEffect, useState } from "react";


export default function PackageGameTypeTime() {
    const [isLoading, setIsLoading] = useState(true);
    const [PackageGameTypeTimes, setPackageGameTypeTimes] = useState<IPackageGameTypeTime[]>([]);

    const loadData = async () => {
        const response = await PackageGameTypeTimeService.getAll();
        if (response.data) {
            setPackageGameTypeTimes(response.data);
            setIsLoading(false);
        }
    }
    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Packages - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Packages</h1>
            <p>
                <Link className="display-6 text-dark" href="/ContestAdmin/PackageGameTypeTime/Create">Create New</Link>
            </p>
            <table className="table table-striped table-hover">
                <thead className="thead-dark">
                    <tr>
                        <th>
                            Name
                        </th>
                        <th>
                            Game Type
                        </th>
                        <th>
                            Part of the whole 
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {PackageGameTypeTimes.map((item) =>
                        <tr key={item.id}>
                            <td>
                                {item.packageGtName}
                            </td>
                            <td>
                                {item.gameType.gameTypeName}
                            </td>
                            <td>
                                {item.times}
                            </td>
                            <td>
                                <Link className="btn btn-primary" href={`/ContestAdmin/PackageGameTypeTime/Edit/${item.id}`}>Edit</Link>
                                <Link className="btn btn-danger" href={`/ContestAdmin/PackageGameTypeTime/Delete/${item.id}`}>Delete</Link>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </>
    );
}

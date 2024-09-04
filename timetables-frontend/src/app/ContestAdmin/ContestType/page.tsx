"use client"

import { IContestType } from "@/domain/IContestType";
import ContestTypeService from "@/services/ContestTypeService";
import Link from "next/link";
import React, { useContext, useEffect, useState } from "react";


export default function ContestType() {
    const [isLoading, setIsLoading] = useState(true);
    const [contestTypes, setContestTypes] = useState<IContestType[]>([]);

    const loadData = async () => {
        const response = await ContestTypeService.getAll();
        if (response.data) {
            setContestTypes(response.data);
            setIsLoading(false);
        }
    }

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>ContestType - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Contest Types</h1>
            <p>
                <Link className="display-6 text-dark" href="/ContestAdmin/ContestType/Create">Create New</Link>
            </p>
            <br />
            <table className="table table-striped table-hover">
                <thead className="thead-dark">
                    <tr>
                        <th>
                            Name
                        </th>
                        <th>
                            Description
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {contestTypes.map((item) =>
                        <tr key={item.id}>
                            <td>
                                {item.contestTypeName}
                            </td>
                            <td>
                                {item.description}
                            </td>
                            <td>
                                <Link className="btn btn-primary" href={`/ContestAdmin/ContestType/Edit/${item.id}`}>Edit</Link>
                                <Link className="btn btn-danger" href={`/ContestAdmin/ContestType/Delete/${item.id}`}>Delete</Link>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </>
    );
}

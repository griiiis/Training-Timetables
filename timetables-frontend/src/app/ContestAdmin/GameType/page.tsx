"use client"
import { IGameType } from "@/domain/IGameType";
import GameTypeService from "@/services/GameTypeService";
import Link from "next/link";
import React, { useEffect, useState } from "react";

export default function GameType() {
    const [isLoading, setIsLoading] = useState(true);
    const [gameTypes, setGameTypes] = useState<IGameType[]>([]);

    const loadData = async () => {
        const response = await GameTypeService.getAll();
        if (response.data) {
            setGameTypes(response.data);
            setIsLoading(false);
        }
    }

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Game Types - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Game Types</h1>
            <p>
                <Link  className="display-6 text-dark" href="/ContestAdmin/GameType/Create">Create New</Link>
            </p>
            <table className="table table-striped table-hover">
                <thead className="thead-dark">
                    <tr>
                        <th>
                            Game Type Name
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {gameTypes.map((item) =>
                        <tr key={item.id}>
                            <td>
                                {item.gameTypeName}
                            </td>
                            <td>
                                <Link className="btn btn-primary" href={`/ContestAdmin/GameType/Edit/${item.id}`}>Edit</Link>
                                <Link className="btn btn-danger"href={`/ContestAdmin/GameType/Delete/${item.id}`}>Delete</Link>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </>
    );
}

"use client"
import { IContest } from "@/domain/IContest";
import ContestService from "@/services/ContestService";
import GameService from "@/services/GameService";
import UserContestPackageService from "@/services/UserContestPackageService";
import Link from "next/link";
import React from "react";
import {useEffect, useState } from "react";

export default function Contest() {
    const [isLoading, setIsLoading] = useState(true);
    const [eachContest, setEachContest] = useState<EachContest[]>([]);

    interface EachContest {
        contest: IContest,
        anyGames: boolean,
        anyTeams: boolean
    }

    const loadData = async () => {
        const contestresponse = await ContestService.getAllOwnerContests();
        console.log(contestresponse.data)

        if (contestresponse.data) {
            let array = Array<EachContest>();
            for (const element of contestresponse.data) {
                const anyGamesResponse = await GameService.anyContestGames(element.id);
                const anyTeamsResponse = await UserContestPackageService.anyTeams(element.id);

                if (contestresponse.data) {
                    const newEachContet: EachContest = {
                        contest: element,
                        anyGames: anyGamesResponse.data!,
                        anyTeams: anyTeamsResponse.data!
                    }
                    array.push(newEachContet);
                };
            };
            setEachContest(array);
            setIsLoading(false);
        }
    }

    useEffect(() => {
        loadData();
     }, []);

    if (isLoading) return (<h1>Contests - LOADING</h1>)

    return (
        <>
            <h1 className="middle">My Contests</h1>
            <p>
                <Link className="display-6 text-dark" href="/ContestAdmin/Contest/Create">Create New</Link>
            </p>
            <br />
            <div className="container">
                <table className="table table-hover">
                    <thead className="thead-dark">
                        <tr>
                            <th scope="col">Name</th>
                            <th scope="col">From</th>
                            <th scope="col">Until</th>
                            <th scope="col">Total Hours</th>
                            <th scope="col">Type</th>
                            <th scope="col">Location Name</th>
                            <th scope="col"></th>
                        </tr>
                    </thead>
                    <tbody>
                        {eachContest.map((item) =>
                            <React.Fragment key={item.contest.id}>
                                <tr>
                                    <td>
                                        {item.contest.contestName}
                                    </td>
                                    <td>
                                        {item.contest.from.substring(0, 10)}
                                    </td>
                                    <td>
                                        {item.contest.until.substring(0, 10)}
                                    </td>
                                    <td>
                                        {item.contest.totalHours}
                                    </td>
                                    <td>
                                        {item.contest.contestType.contestTypeName}
                                    </td>
                                    <td>
                                        {item.contest.location.locationName}
                                    </td>
                                    <td>
                                        <Link className="btn btn-success" href={`/ContestAdmin/Contest/Overview/${item.contest.id}`}>Overview</Link>
                                
                                        <Link className="btn btn-success" href={`/ContestAdmin/Game/Create/${item.contest.id}`}>Create Games</Link>
                                
                                    {item.anyGames ?
                                        
                                            <Link className="btn btn-success" href={`/ContestAdmin/Game/${item.contest.id}`}>Games</Link>
                                         :
                                        <>
                                        </>
                                    }
                                    
                                        <Link className="btn btn-success" href={`/ContestAdmin/AppUser/${item.contest.id}`}>Add Trainers</Link>
                                    
                                    {item.anyTeams ?
                                        <>
                                            
                                                <Link className="btn btn-primary" href={`/ContestAdmin/Contest/Edit/${item.contest.id}`}>Edit</Link>
                                            
                                                <Link className="btn btn-danger" href={`/ContestAdmin/Contest/Delete/${item.contest.id}`}>Delete</Link>
                                            
                                        </>
                                        :
                                        <></>
                                    }
                                    </td>
                                </tr>
                            </React.Fragment>
                        )}
                    </tbody>
                </table>
            </div>
        </>
    );
}

"use client"
import ContestService from "@/services/ContestService";
import { IMyContestsDTO } from "@/domain/DTOs/Contests/IMyContestsDTO";
import Link from "next/link";
import React from "react";
import { useEffect, useState } from "react";


export default function Index() {
    const [myContests, setMyContests] = useState<IMyContestsDTO>()
    const [isLoading, setIsLoading] = useState(true);

    const loadData = async () => {
        const allContestsResponse = await ContestService.getUserContests();
        if (allContestsResponse.data) {
            setMyContests(allContestsResponse.data);
            setIsLoading(false)
        }
    }

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>MY Contests - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Contests</h1>
            <br />

            {myContests!.currentContestsDTO.length > 0 && myContests!.currentContestsDTO[0] !== null  && (
                <div className="ended-contests">
                    <h2 className="section-title">Current Contests</h2>
                    <div className="row">
                        {myContests!.currentContestsDTO.map(contestDTO => {
                            return (
                                <React.Fragment key={`${contestDTO!.contestId}`}>
                                    <div className="col-md-6 mb-4">
                                        <div className="card ended-contest-card">
                                            <div className="card-body">
                                                <div className="row">
                                                    <div className="col-md-6">
                                                        {contestDTO.ifTrainer ? (<h2 className="contest-name">{contestDTO!.contestName} - Trainer</h2>)
                                                            : (
                                                                <h2 className="contest-name">{contestDTO!.contestName}</h2>
                                                            )}
                                                        <p className="contest-duration">{new Date(contestDTO!.from).toDateString() + ' ' + new Date(contestDTO!.from).toTimeString().substring(0, 8) + ' -'} <br></br>
                                                            {new Date(contestDTO!.until).toDateString() + ' ' + new Date(contestDTO!.until).toTimeString().substring(0, 8)}</p>
                                                        {contestDTO.ifTrainer ? (
                                                            <>
                                                                {contestDTO.ifTrainer && (
                                                                    <div>
                                                                        {contestDTO!.gameTypesDTOs.map(gameTypeDTO => {

                                                                            const selectedTitles = contestDTO!.rolePreferencesDTOs
                                                                                .filter(rolePreference => rolePreference.gameTypeId === gameTypeDTO.gameTypeId)
                                                                                .sort((a, b) => a!.levelTitle.localeCompare(b.levelTitle))
                                                                                .map(pref => pref.levelTitle);

                                                                            return (
                                                                                <div key={gameTypeDTO.gameTypeId} className="contest-detail">
                                                                                    <strong>{gameTypeDTO.gameTypeName}:</strong>
                                                                                    <p>{selectedTitles.length ? selectedTitles.join(", ") : "None"}</p>
                                                                                </div>
                                                                            );
                                                                        })}
                                                                    </div>
                                                                )}
                                                            </>
                                                        ) : (
                                                            <>
                                                                <p className="contest-detail">
                                                                    <strong>Level:</strong> {contestDTO!.levelTitle}
                                                                </p>
                                                                <p className="contest-detail">
                                                                    <strong>Game Type:</strong> {contestDTO!.gameTypeName}
                                                                </p>
                                                                <p className="contest-detail">
                                                                    <strong>Package:</strong> {contestDTO!.packageName}
                                                                </p>
                                                            </>
                                                        )}
                                                        {contestDTO!.anyGames && (
                                                            <div className="contest-actions">
                                                                <Link className="btn btn-primary" href={`/Games/${contestDTO!.contestId}`}>Games</Link>
                                                            </div>
                                                        )}

                                                    </div>
                                                    {!contestDTO!.ifTrainer ? (
                                                        <div className="col-md-6">

                                                            <h4>Teammates</h4>
                                                            <ul className="team-members">
                                                                {contestDTO!.packagesDTOs.map(userPackage => (
                                                                    <li key={userPackage.packageId}>
                                                                        {userPackage.firstName} {userPackage.lastName}
                                                                    </li>
                                                                ))}
                                                            </ul>
                                                        </div>

                                                    ) : (<></>)}

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </React.Fragment>
                            );
                        }
                        )}

                    </div>
                </div>
            )}
            {myContests!.comingContestsDTO.length > 0 && myContests!.comingContestsDTO[0] !== null && (
                <div className="ended-contests">
                    <h2 className="section-title">Coming Contests</h2>
                    <div className="row">
                        {myContests!.comingContestsDTO.map(contestDTO => {
                            return (
                                <React.Fragment key={`${contestDTO!.contestId}`}>
                                    <div className="col-md-6 mb-4">
                                        <div className="card ended-contest-card">
                                            <div className="card-body">
                                                <div className="row">
                                                    <div className="col-md-6">
                                                        {contestDTO.ifTrainer ? (<h2 className="contest-name">{contestDTO!.contestName} - Trainer</h2>)
                                                            : (
                                                                <h2 className="contest-name">{contestDTO!.contestName}</h2>
                                                            )}
                                                        <p className="contest-duration">{new Date(contestDTO.from).toDateString() + ' ' + new Date(contestDTO.from).toTimeString().substring(0, 8) + ' -'} <br></br>
                                                            {new Date(contestDTO.until).toDateString() + ' ' + new Date(contestDTO.until).toTimeString().substring(0, 8)}</p>

                                                        {contestDTO.ifTrainer ? (
                                                            <div className="contest-actions">
                                                                <>
                                                                    {contestDTO.ifTrainer && (
                                                                        <div>
                                                                            {contestDTO!.gameTypesDTOs.map(gameTypeDTO => {

                                                                                const selectedTitles = contestDTO!.rolePreferencesDTOs
                                                                                .filter(rolePreference => rolePreference.gameTypeId === gameTypeDTO.gameTypeId)
                                                                                .sort((a, b) => a!.levelTitle.localeCompare(b.levelTitle))
                                                                                .map(pref => pref.levelTitle);

                                                                                return (
                                                                                    <div key={gameTypeDTO.gameTypeId} className="contest-detail">
                                                                                        <strong>{gameTypeDTO.gameTypeName}:</strong>
                                                                                        <p>{selectedTitles.length ? selectedTitles.join(", ") : "None"}</p>
                                                                                    </div>
                                                                                );
                                                                            })}
                                                                        </div>
                                                                    )}
                                                                    <div className="contest-actions">
                                                                        {contestDTO.anyGames && (
                                                                            <Link className="btn btn-primary" href={`/Games/${contestDTO.contestId}`}>Games</Link>
                                                                        )}
                                                                        <Link className="btn btn-primary" href={`/RolePreference/${contestDTO.contestId}`}>Trainer Prefrerences</Link>
                                                                    </div>
                                                                </>
                                                            </div>) : (
                                                            <>
                                                                <p className="contest-detail">
                                                                    <strong>Level:</strong> {contestDTO.levelTitle}
                                                                </p>
                                                                <p className="contest-detail">
                                                                    <strong>Game Type:</strong> {contestDTO!.gameTypeName}
                                                                </p>
                                                                <p className="contest-detail">
                                                                    <strong>Package:</strong> {contestDTO!.packageName}
                                                                </p>
                                                                <div className="contest-actions">
                                                                    {contestDTO.anyGames && (
                                                                        <Link className="btn btn-primary" href={`/Games/${contestDTO.contestId}`}>Games</Link>
                                                                    )}
                                                                    <Link className="btn btn-success" href={`/UserContestPackage/${contestDTO.contestId}/${contestDTO?.teamId}`}>Add Teammates</Link>
                                                                    <Link className="btn btn-info" href={`/Preferences/${contestDTO!.contestId}/${contestDTO?.teamId}`}>Preferences</Link>
                                                                </div>
                                                            </>
                                                        )}
                                                    </div>
                                                    {!contestDTO.ifTrainer ? (
                                                        <div className="col-md-6">

                                                            <h4>Teammates</h4>
                                                            {contestDTO!.packagesDTOs.map(userPackage => (
                                                                <li key={userPackage.packageId}>
                                                                    {userPackage.firstName} {userPackage.lastName}
                                                                </li>
                                                            ))}
                                                        </div>

                                                    ) : (<></>)}


                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </React.Fragment>
                            );
                        }
                        )}
                    </div>
                </div>
            )}
        </>
    );

}
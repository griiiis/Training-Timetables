"use client"
import { IContest } from "@/domain/IContest";
import { IGameType } from "@/domain/IGameType";
import { ILevel } from "@/domain/ILevel";
import { IPackageGameTypeTime } from "@/domain/IPackageGameTypeTime";
import { IRolePreference } from "@/domain/IRolePreference";
import { IUserContestPackage } from "@/domain/IUserContestPackage";
import ContestService from "@/services/ContestService";
import GameService from "@/services/GameService";
import GameTypeService from "@/services/GameTypeService";
import LevelService from "@/services/LevelService";
import PackageGameTypeTimeService from "@/services/PackageGameTypeTimeService";
import RolePreferenceService from "@/services/RolePreferenceService";
import UserContestPackageService from "@/services/UserContestPackageService";
import { IUserInfo } from "@/state/AppContext";
import Link from "next/link";
import React from "react";
import { useEffect, useState } from "react";


export default function Index() {

    interface MyContestsModel {
        ComingContests: Array<ContestModel | null>,
        CurrentContests: Array<ContestModel | null>,
        IfTeacher: boolean,
        RolePreferences: Array<IRolePreference[]>
    }

    interface ContestModel {
        Contest: IContest,
        AnyGames: boolean,
        UserContestPackage: IUserContestPackage,
        UserContestPackages: Array<IUserContestPackage>,
        GameTypes: Array<IGameType>,
        Level: ILevel,
        GameType: IGameType,
        PackageGameTypeTime: IPackageGameTypeTime
    }

    const [myContests, setMyContests] = useState<MyContestsModel>()
    const [isLoading, setIsLoading] = useState(true);

    const loadData = async () => {

        const userInfo : IUserInfo = JSON.parse(localStorage.getItem("userInfo")!)

        const allContestsResponse = await ContestService.getUserContests();
        const ifTeacher = (userInfo?.role === "Treener");
        const RolePreferences = Array<IRolePreference[]>([]);
        if (ifTeacher) {
            const rolePreferences = await RolePreferenceService.getAll();
            if (rolePreferences.data) {
                RolePreferences.push(rolePreferences.data);
            }
        }

        if (allContestsResponse.data) {

            // Current contests
            const currentContests: Array<ContestModel | null> = await Promise.all(allContestsResponse.data!
                .filter(contest => new Date(contest.from) < new Date() && new Date(contest.until) > new Date())
                .map(async contest => {
                    const userContestPackage = (await UserContestPackageService.getUserContestPackage(contest.id));
                    if (userContestPackage.data) {
                        const GetContest = await ContestService.getContestInformation(contest.id);
                        const AnyGames = await GameService.anyContestGames(contest.id)!;
                        const UserContestPackage = userContestPackage.data;
                        const UserContestPackages = await UserContestPackageService.getContestTeammates(contest.id, UserContestPackage.teamId);
                        const GameTypes = await GameTypeService.getCurrentContestGameTypes(contest.id);
                        const Level = await LevelService.getLevelForAll(UserContestPackage.levelId);
                        const GameType = await GameTypeService.getGameTypeForAll(UserContestPackage.packageGameTypeTime!.gameType.id);
                        const PackageGameTypeTime = await PackageGameTypeTimeService.getPackageGameTypeTimeForAll(UserContestPackage.packageGameTypeTime!.id);
                        if (GetContest.data && AnyGames.data && UserContestPackages.data && GameTypes.data && Level.data && GameType.data && PackageGameTypeTime.data) {
                            return {
                                Contest: GetContest.data,
                                AnyGames: AnyGames.data,
                                UserContestPackage: userContestPackage.data,
                                UserContestPackages: UserContestPackages.data,
                                GameTypes: GameTypes.data,
                                Level: Level.data,
                                GameType: GameType.data,
                                PackageGameTypeTime: PackageGameTypeTime.data
                            };
                        }
                    }
                    return null;
                }));


            // Coming contests
            const comingContests: Array<ContestModel | null> = await Promise.all(allContestsResponse.data!
                .filter(contest => new Date(contest.from) > new Date())
                .map(async contest => {
                    const userContestPackage = await UserContestPackageService.getUserContestPackage(contest.id);
                    if (userContestPackage.data) {
                        const GetContest = await ContestService.getContestInformation(contest.id);
                        const AnyGames = await GameService.anyContestGames(contest.id);
                        const UserContestPackage = userContestPackage.data;
                        const UserContestPackages = await UserContestPackageService.getContestTeammates(contest.id, UserContestPackage.teamId);
                        const GameTypes = await GameTypeService.getCurrentContestGameTypes(contest.id);
                        const Level = await LevelService.getLevelForAll(UserContestPackage.levelId);
                        const GameType = await GameTypeService.getGameTypeForAll(UserContestPackage.packageGameTypeTime!.gameType.id);
                        const PackageGameTypeTime = await PackageGameTypeTimeService.getPackageGameTypeTimeForAll(UserContestPackage.packageGameTypeTime!.id);
                        if (GetContest.data && UserContestPackages.data && GameTypes.data && Level.data && GameType.data && PackageGameTypeTime.data) {
                            return {
                                Contest: GetContest.data,
                                AnyGames: AnyGames.data!,
                                UserContestPackage: userContestPackage.data,
                                UserContestPackages: UserContestPackages.data,
                                GameTypes: GameTypes.data,
                                Level: Level.data,
                                GameType: GameType.data,
                                PackageGameTypeTime: PackageGameTypeTime.data
                            };
                        }
                    }
                    return null;
                }));
            setMyContests({
                ComingContests: comingContests,
                CurrentContests: currentContests,
                IfTeacher: ifTeacher,
                RolePreferences: RolePreferences
            });
            setIsLoading(false)
        }
    }


    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>MY Contests - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Contests</h1>
            <br />

            {myContests!.CurrentContests.length > 0 && myContests!.CurrentContests[0] !== null  && (
                <div className="ended-contests">
                    <h2 className="section-title">Current Contests</h2>
                    <div className="row">
                        {myContests!.CurrentContests.map(contest => {
                            return (
                                <React.Fragment key={`${contest!.Contest.id}`}>
                                    <div className="col-md-6 mb-4">
                                        <div className="card ended-contest-card">
                                            <div className="card-body">
                                                <div className="row">
                                                    <div className="col-md-6">
                                                        {myContests!.IfTeacher ? (<h2 className="contest-name">{contest!.Contest.contestName} - Trainer</h2>)
                                                            : (
                                                                <h2 className="contest-name">{contest!.Contest.contestName}</h2>
                                                            )}
                                                        <p className="contest-duration">{new Date(contest!.Contest.from).toDateString() + ' ' + new Date(contest!.Contest.from).toTimeString().substring(0, 8) + ' -'} <br></br>
                                                            {new Date(contest!.Contest.until).toDateString() + ' ' + new Date(contest!.Contest.until).toTimeString().substring(0, 8)}</p>
                                                        {myContests!.IfTeacher ? (
                                                            <>
                                                                {myContests!.IfTeacher && (
                                                                    <div>
                                                                        {contest!.GameTypes.map(gameType => {
                                                                            const selectedTitles = myContests!.RolePreferences
                                                                                .flatMap(preferences => preferences
                                                                                    .filter(pref => pref.contestId === contest!.Contest.id && pref.gameTypeId === gameType.id))
                                                                                .sort((a, b) => a!.level.title.localeCompare(b.level!.title))
                                                                                .map(pref => pref.level!.title);
                                                                            return (
                                                                                <div key={gameType.id} className="contest-detail">
                                                                                    <strong>{gameType.gameTypeName}:</strong>
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
                                                                    <strong>Level:</strong> {contest!.Level.title}
                                                                </p>
                                                                <p className="contest-detail">
                                                                    <strong>Game Type:</strong> {contest!.PackageGameTypeTime.gameType.gameTypeName}
                                                                </p>
                                                                <p className="contest-detail">
                                                                    <strong>Package:</strong> {contest!.PackageGameTypeTime.packageGtName}
                                                                </p>
                                                            </>
                                                        )}
                                                        {contest!.AnyGames && (
                                                            <div className="contest-actions">
                                                                <Link className="btn btn-primary" href={`/Games/${contest!.Contest.id}`}>Games</Link>
                                                            </div>
                                                        )}

                                                    </div>
                                                    {!myContests!.IfTeacher ? (
                                                        <div className="col-md-6">

                                                            <h4>Teammates</h4>
                                                            <ul className="team-members">
                                                                {contest!.UserContestPackages.map(userPackage => (
                                                                    <li key={userPackage.id}>
                                                                        {userPackage.appUser.firstName} {userPackage.appUser.lastName}
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
            {myContests!.ComingContests.length > 0 && myContests!.ComingContests[0] !== null && (
                <div className="ended-contests">
                    <h2 className="section-title">Comings Contests</h2>
                    <div className="row">
                        {myContests!.ComingContests.map(contest => {
                            return (
                                <React.Fragment key={`${contest!.Contest.id}`}>
                                    <div className="col-md-6 mb-4">
                                        <div className="card ended-contest-card">
                                            <div className="card-body">
                                                <div className="row">
                                                    <div className="col-md-6">
                                                        {myContests!.IfTeacher ? (<h2 className="contest-name">{contest!.Contest.contestName} - Trainer</h2>)
                                                            : (
                                                                <h2 className="contest-name">{contest!.Contest.contestName}</h2>
                                                            )}
                                                        <p className="contest-duration">{new Date(contest!.Contest.from).toDateString() + ' ' + new Date(contest!.Contest.from).toTimeString().substring(0, 8) + ' -'} <br></br>
                                                            {new Date(contest!.Contest.until).toDateString() + ' ' + new Date(contest!.Contest.until).toTimeString().substring(0, 8)}</p>

                                                        {myContests!.IfTeacher ? (
                                                            <div className="contest-actions">
                                                                <>
                                                                    {myContests!.IfTeacher && (
                                                                        <div>
                                                                            {contest!.GameTypes.map(gameType => {
                                                                                const selectedTitles = myContests!.RolePreferences
                                                                                    .flatMap(preferences => preferences
                                                                                        .filter(pref => pref.contestId === contest!.Contest.id && pref.gameTypeId === gameType.id))
                                                                                    .sort((a, b) => a!.level.title.localeCompare(b.level!.title))
                                                                                    .map(pref => pref.level!.title);
                                                                                return (
                                                                                    <div key={gameType.id} className="contest-detail">
                                                                                        <strong>{gameType.gameTypeName}:</strong>
                                                                                        <p>{selectedTitles.length ? selectedTitles.join(", ") : "None"}</p>
                                                                                    </div>
                                                                                );
                                                                            })}
                                                                        </div>
                                                                    )}
                                                                    <div className="contest-actions">
                                                                        {contest!.AnyGames && (
                                                                            <Link className="btn btn-primary" href={`/Games/${contest!.Contest.id}`}>Games</Link>
                                                                        )}
                                                                        <Link className="btn btn-primary" href={`/RolePreference/${contest!.Contest.id}`}>Trainer Prefrerences</Link>
                                                                    </div>
                                                                </>
                                                            </div>) : (
                                                            <>
                                                                <p className="contest-detail">
                                                                    <strong>Level:</strong> {contest!.Level.title}
                                                                </p>
                                                                <p className="contest-detail">
                                                                    <strong>Game Type:</strong> {contest!.PackageGameTypeTime.gameType.gameTypeName}
                                                                </p>
                                                                <p className="contest-detail">
                                                                    <strong>Package:</strong> {contest!.PackageGameTypeTime.packageGtName}
                                                                </p>
                                                                <div className="contest-actions">
                                                                    {contest!.AnyGames && (
                                                                        <Link className="btn btn-primary" href={`/Games/${contest!.Contest.id}`}>Games</Link>
                                                                    )}
                                                                    <Link className="btn btn-success" href={`/UserContestPackage/${contest!.Contest.id}/${contest?.UserContestPackage.teamId}`}>Add Teammates</Link>
                                                                    <Link className="btn btn-info" href={`/Preferences/${contest!.Contest.id}/${contest?.UserContestPackage.teamId}`}>Preferences</Link>
                                                                </div>
                                                            </>
                                                        )}
                                                    </div>
                                                    {!myContests!.IfTeacher ? (
                                                        <div className="col-md-6">

                                                            <h4>Teammates</h4>
                                                            {contest!.UserContestPackages.map(userPackage => (
                                                                <li key={userPackage.id}>
                                                                    {userPackage.appUser.firstName} {userPackage.appUser.lastName}
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
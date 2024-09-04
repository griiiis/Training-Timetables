"use client"
import { IContest } from "@/domain/IContest";
import { IGame } from "@/domain/IGame";
import { IGameType } from "@/domain/IGameType";
import { ITime } from "@/domain/ITime";
import { IUserContestPackage } from "@/domain/IUserContestPackage";
import ContestService from "@/services/ContestService";
import GameService from "@/services/GameService";
import GameTypeService from "@/services/GameTypeService";
import TimeService from "@/services/TimeService";
import UserContestPackageService from "@/services/UserContestPackageService";
import { IUserInfo } from "@/state/AppContext";
import { useParams } from "next/navigation";
import React, { useEffect, useState } from "react";

export default function Game() {
    let { contestId } = useParams();
    const [isLoading, setIsLoading] = useState(true);
    const [games, setGames] = useState<IGame[]>([]);
    const [contest, setContest] = useState<IContest>();
    const [gameTypes, setGameTypes] = useState<IGameType[]>([]);
    const [times, setTimes] = useState<ITime[]>([]);
    const [allDays, setAllDays] = useState(Array<Date>());
    const [trainersEmails, setTrainersEmails] = useState([""]);
    const [trainers, setTrainers] = useState<IUserContestPackage[]>([]);
    const [players, setPlayers] = useState<IUserContestPackage[]>([]);

    const loadData = async () => {
        const gamesResponse = await GameService.getUserContestGames(contestId.toString());
        const contestResponse = await ContestService.getContestInformation(contestId.toString())
        const gameTypesResponse = await GameTypeService.getCurrentContestGameTypes(contestId.toString())
        const timesResponse = await TimeService.getCurrentContestTimes(contestId.toString());
        const trainersResponse = await UserContestPackageService.getContestTeachers(contestId.toString());
        const playersResponse = await UserContestPackageService.getContestUsersWithoutTrainers(contestId.toString());
        const userContestPackage = (await UserContestPackageService.getUserContestPackage(contestId.toString()));
        if (gamesResponse.data && contestResponse.data && gameTypesResponse.data && timesResponse.data && trainersResponse.data && playersResponse.data) {
            const userInfo : IUserInfo = JSON.parse(localStorage.getItem("userInfo")!)
            let players;
            let trainers;
            let gameIds;
            const teamId = userContestPackage.data!.teamId;
            //TODO:
            
            if (userInfo?.role === "Treener") {
                trainers = trainersResponse.data!.filter(e => e.teamId === teamId);
                gameIds = trainers!.flatMap(e => e.team.teamGames.map(e => e.gameId));
                players = playersResponse.data!;
            } else {
                players = playersResponse.data!;;
                gameIds = players!.flatMap(e => e.team.teamGames.map(e => e.gameId));
                trainers = trainersResponse.data;
            }
            const finalGames = gamesResponse.data!.filter(e => gameIds.includes(e.id))
            setGames(finalGames!);
            setContest(contestResponse.data);
            setGameTypes(gameTypesResponse.data);
            setTimes(timesResponse.data);
            setTrainers(trainersResponse.data)
            setPlayers(players!)
            setTrainersEmails(trainersResponse.data.map(e => e.appUser.email))
            setIsLoading(false);
        }
    }

    useEffect(() => {
        if (!isLoading) {
            const days = [];
            for (var date = new Date(contest!.from); date < new Date(contest!.until); date.setDate(date.getDate() + 1)) {
                days.push(new Date(date));
            }
            setAllDays(days);
        }
    }, [isLoading, contest])

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Games - LOADING</h1>)

    return (
        <>
            <div>
                <h1 className="middle">{contest && `${contest.contestName} Games`}</h1>
                <br />
                <div className="container">
                    {allDays.map(day => {
                        let gamesForDay = games.filter(game => new Date(game.from).toLocaleDateString() === new Date(day).toLocaleDateString())
                        if (gamesForDay.length > 0) {
                            return (
                                <React.Fragment key={day.toString()}>
                                    <hr />
                                    <div className="row">
                                        <div className="col">
                                            <h1>{new Date(day).toDateString()}</h1>
                                            <br />
                                        </div>
                                    </div>
                                    {times.map(time => {
                                        const gamesForTime = gamesForDay.filter(game => new Date(game.from).toLocaleTimeString('en-IT', { hour12: false }) === time.from);
                                        if (gamesForTime.length > 0) {
                                            return (
                                                <React.Fragment key={`${day}-${time.from}`}>
                                                    <div className="row">
                                                        <div className="col">
                                                            <h2>{`${time.from} - ${time.until}`}</h2>
                                                        </div>
                                                    </div>
                                                    <div className="row">
                                                        {gameTypes.map(gameType => {
                                                            const gamesOfType = gamesForTime.filter(game =>
                                                                game.gameTypeId === gameType.id
                                                            );
                                                            if (gamesOfType.length > 0) {
                                                                return (
                                                                    <div className="col-md-4" key={`${day}-${time.from}-${gameType.id}`}>
                                                                        <div className="card mb-4">
                                                                            <div className="card-body">
                                                                                <h3 className="card-title">{gameType.gameTypeName}</h3>
                                                                                {gamesOfType.map(game => (
                                                                                    <div className="game-details" key={game.id}>
                                                                                        <h4>{`${game.court.courtName} - ${game.level.title}`}</h4>
                                                                                        <p>
                                                                                            <b>Trainer: </b>
                                                                                            {trainers
                                                                                                .filter(trainer =>
                                                                                                    trainer.team && trainer.team.teamGames.some(g =>
                                                                                                        g.gameId === game.id
                                                                                                    )
                                                                                                )
                                                                                                .map(trainer => `${trainer.appUser.firstName} ${trainer.appUser.lastName}`)
                                                                                                .join(", ")
                                                                                            };
                                                                                        </p>
                                                                                        <p>
                                                                                            <b>Players:</b> {
                                                                                                players.filter(player =>
                                                                                                    player.team.teamGames.some(teamGame =>
                                                                                                        teamGame.gameId === game.id && 
                                                                                                        !trainersEmails.includes(player.appUser.email)
                                                                                                    )
                                                                                                ).map(user => `${user.appUser.firstName} ${user.appUser.lastName}`).join(", ")};

                                                                                        </p>
                                                                                    </div>
                                                                                ))}
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                );
                                                            } else {
                                                                return null;
                                                            }
                                                        })}
                                                    </div>
                                                </React.Fragment>
                                            );
                                        } else {
                                            return null;
                                        }
                                    })}
                                </React.Fragment>
                            );
                        } else {
                            return null;
                        }
                    })}
                </div>
            </div>
        </>
    );
}
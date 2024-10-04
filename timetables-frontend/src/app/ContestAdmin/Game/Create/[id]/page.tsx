"use client"
import { IContest } from "@/domain/IContest";
import { ICourt } from "@/domain/ICourt";
import { IGameType } from "@/domain/IGameType";
import { ILevel } from "@/domain/ILevel";
import { ITeam } from "@/domain/ITeam";
import { ITime } from "@/domain/ITime";
import { IUserContestPackage } from "@/domain/IUserContestPackage";
import ContestService from "@/services/ContestService";
import CourtService from "@/services/CourtService";
import GameService from "@/services/GameService";
import GameTypeService from "@/services/GameTypeService";
import LevelService from "@/services/LevelService";
import TeamService from "@/services/TeamService";
import TimeService from "@/services/TimeService";
import UserContestPackageService from "@/services/UserContestPackageService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useContext, useEffect, useState } from "react";

export default function Create() {
    interface TimesData {
        Date: string,
        SelectedTimesList: Array<string>;
    }

    let { id } = useParams();
    const router = useRouter();
    const [from, setFrom] = useState('');
    const [until, setUntil] = useState('');
    const [peoplePerCourtInputs, setPeoplePerCourtInputs] = useState([0]);
    const [selectedLevelIds, setSelectedLevelIds] = useState(Array(Array()));
    const [selectedCourtIds, setSelectedCourtIds] = useState(Array(Array()));
    const [selectedTimeIds, setSelectedTimeIds] = useState(Array<TimesData>());
    const [selectedTimesByDay, setSelectedTimesByDay] = useState({});
    const [users, setUsers] = useState<IUserContestPackage[]>([]);
    const [trainers, setTrainers] = useState<IUserContestPackage[]>([]);
    const [teacherIds, setTeacherIds] = useState(Array(Array()));
    const [contest, setContest] = useState<IContest>();
    const [courts, setCourts] = useState<ICourt[]>([]);
    const [gameTypes, setGameTypes] = useState<IGameType[]>([]);
    const [times, setTimes] = useState<ITime[]>([]);
    const [levels, setLevels] = useState<ILevel[]>([]);
    const [teams, setTeams] = useState<ITeam[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [validationError, setValidationError] = useState("");

    const CreateNewGame = async () => {
        const createGamesData = {
            selectedLevelIds: selectedLevelIds,
            selectedCourtIds: selectedCourtIds,
            selectedTimesIds: selectedTimeIds,
            peoplePerCourtInputs: peoplePerCourtInputs,
            selectedTrainersIds : teacherIds
        }
    

        const response = await GameService.postGame(id.toString(), createGamesData);
        if (response.data) {
            router.push("/ContestAdmin/Contest");
        }
        if (response.errors && response.errors.length > 0) {
            setValidationError(response.errors[0]);
        }
    };

    useEffect(() => { loadData() }, []);
    const loadData = async () => {
        const contestResponse = await ContestService.getContest(id.toString());
        const courtResponse = await CourtService.getAll();
        const gameTypeResponse = await GameTypeService.getCurrentContestGameTypes(id.toString());
        const timesResponse = await TimeService.getCurrentContestTimes(id.toString());
        const levelResponse = await LevelService.getCurrentContestLevels(id.toString());
        const userTeamResponse = await UserContestPackageService.getContestUsersWithoutTrainers(id.toString());
        const trainersResponse = await UserContestPackageService.getContestTeachers(id.toString());
        const teamsResponse = await TeamService.getCurrentContestTeams(id.toString());

        if (courtResponse.data && gameTypeResponse.data && levelResponse.data && timesResponse.data && userTeamResponse.data && contestResponse.data && trainersResponse.data && teamsResponse.data) {
            setCourts(courtResponse.data);
            setGameTypes(gameTypeResponse.data);
            setTimes(timesResponse.data);
            setLevels(levelResponse.data);
            setUsers(userTeamResponse.data)
            setContest(contestResponse.data)
            setTrainers(trainersResponse.data)
            setTeams(teamsResponse.data)
            setIsLoading(false);
        };
    }

    useEffect(() => {
        const startDate = new Date(from);
        const endDate = new Date(until);
        endDate.setDate(endDate.getDate() + 1);
        const differenceInTime = endDate.getTime() - startDate.getTime();
        const differenceInDays = differenceInTime / (1000 * 3600 * 24);
        const generatedBoxes = [];
        for (let i = 0; i < differenceInDays; i++) {
            const box = (
                <div className="form-group" key={i}>
                    <label className="control-label">Day {i + 1}</label>
                    <select className="form-control" multiple onChange={(e) => {
                        const selectedtimes = Array.from(e.target.selectedOptions, option => option.value);
                        setSelectedTimeIds(prevTimeIds => {
                            const updatedTimeIds = [...prevTimeIds];
                            const startDate = new Date(from);
                            startDate.setDate(startDate.getDate() + i);
                            const date = startDate.toISOString().split('T')[0];
                            const TimesData: TimesData = {
                                Date: date,
                                SelectedTimesList: selectedtimes
                            }
                            updatedTimeIds[i] = TimesData;
                            return updatedTimeIds;
                        }
                        );
                    }}>
                        {times.map(time => (
                            <option key={time.id} value={time.id}>
                                {`${time.from.toString()} - ${time.until.toString()}`}
                            </option>
                        ))}
                    </select>
                </div>
            );
            generatedBoxes.push(box);
        }
        setSelectedTimesByDay(generatedBoxes);
    }, [from, until, times]);

    useEffect(() => {
        const nestedLengthSum = selectedTimeIds.reduce((acc, innerArray) => {
            const innerLength = innerArray.SelectedTimesList.length;
            return acc + innerLength;
        }, 0);

        for (let i = 0; i < gameTypes.length; i++) {
            if (selectedTimeIds[0] !== undefined && selectedCourtIds[i] !== undefined) {
                let courtCount = selectedCourtIds[i].length
                let usersCount = users.filter(e => e.team.gameTypeId === gameTypes[i].id).length
                let peoplePerCourt;
                console.log(courtCount)
                if (courtCount === 0) {
                    peoplePerCourt = 0;
                } else {
                    peoplePerCourt = usersCount / (nestedLengthSum * courtCount);
                }
                let peoplePerCourtValue = "peoplePerCourt_" + gameTypes[i].id;
                let divElement = document.getElementById(peoplePerCourtValue);
                divElement!.innerHTML = "Currently People Per Court: " + peoplePerCourt;
            }
        }
    }, [selectedTimeIds, gameTypes, selectedCourtIds, users]);

    if (isLoading) return (<h1>Create new Game - LOADING</h1>);

    return (
        <>
            <h1 className="middle">Create New Games</h1>
            <hr />
            <div className="row">
                <div className="">
                    <form>
                        <div className="text-danger" role="alert">{validationError}</div>
                        <div className="container">
                            <div className={`row row-cols-${gameTypes.length}`}>
                                {gameTypes.map((gameType, index) => (
                                    <div className="col" key={index}>
                                        <div className="col">
                                            <div className="card">
                                                <div className="card-body">
                                                    <h5 className="card-title">{gameTypes[index].gameTypeName}</h5>
                                                    <p className="card-text">Teams Count: {teams.filter(e => e.gameType.id === gameType.id).length}</p>
                                                    <p className="card-text">People Count: {users.filter(e => e.team.gameTypeId == gameType.id).length}</p>
                                                    <div id={`peoplePerCourt_${gameType.id}`}>Currently People Per Court: <span id="peoplePerCourtValue">0</span></div>
                                                    <br />
                                                    <div className="form-group">
                                                        <label>How many people per court</label>
                                                        <input type="number" className="form-control" min="0" onChange={(e) => {
                                                            const value = parseInt(e.target.value);
                                                            setPeoplePerCourtInputs(prevInputs => {
                                                                const updatedInputs = [...prevInputs];
                                                                updatedInputs[index] = value;
                                                                return updatedInputs;
                                                            });
                                                        }} />
                                                    </div>
                                                    <br />
                                                    <div className="form-group">
                                                        <label className="control-label" htmlFor="Levels">Vali tasemed</label>
                                                        <select multiple className="form-control" onChange={(e) => {
                                                            const selectedLevels = Array.from(e.target.selectedOptions, option => option.value);
                                                            setSelectedLevelIds(prevLevelIds => {
                                                                const updatedLevelIds = [...prevLevelIds];
                                                                updatedLevelIds[index] = selectedLevels;
                                                                return updatedLevelIds;
                                                            }
                                                            ); setValidationError("");
                                                        }}
                                                        >{levels.map((level) => {
                                                            return (
                                                                <option key={level.id} value={level.id}>
                                                                    {level.title}
                                                                </option>
                                                            );
                                                        })}</select>
                                                    </div>
                                                    <br />
                                                    <div className="form-group">
                                                        <label className="control-label" htmlFor="Courts">Vali väljakud</label>
                                                        <select id={`selectCourts_${gameType.id}`} multiple className="form-control" onChange={(e) => {
                                                            const selectedCourts = Array.from(e.target.selectedOptions, option => option.value);
                                                            setSelectedCourtIds(prevCourtIds => {
                                                                const updatedCourtIds = [...prevCourtIds];
                                                                updatedCourtIds[index] = selectedCourts;
                                                                return updatedCourtIds;
                                                            }
                                                            ); setValidationError("");
                                                        }}
                                                        >{courts.filter(e => e.gameTypeId === gameType.id).map((court) => {
                                                            return (
                                                                <option key={court.id} value={court.id}>
                                                                    {court.courtName}
                                                                </option>
                                                            );
                                                        })}</select>
                                                    </div>
                                                    <br />
                                                    <div className="form-group">
                                                        <label className="control-label" htmlFor="Teachers">Teachers</label>

                                                        <select multiple className="form-control" onChange={(e) => {
                                                            const selectedTeachers = Array.from(e.target.selectedOptions, option => option.value);
                                                            setTeacherIds(prevTeachersIds => {
                                                                const updatedTeachersIds = [...prevTeachersIds];
                                                                updatedTeachersIds[index] = selectedTeachers;
                                                                return updatedTeachersIds;
                                                            }
                                                            ); setValidationError("");
                                                        }}
                                                        >{trainers.filter(e => e.appUser.rolePreferences.some(c => c.contestId === contest!.id && c.gameTypeId === gameType.id)).map((teacher) => {
                                                            return (
                                                                <option key={teacher.id} value={teacher.id}>
                                                                    {`${teacher.appUser.firstName} ${teacher.appUser.lastName}`}
                                                                </option>
                                                            );
                                                        })}</select>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </div>
                        <br />
                        <div className="container">
                            <div className="row row-cols-6">
                                <div className="col">
                                    <div className="form-group">
                                        <label className="control-label">From</label>
                                        <input id="startDate" type="date" min={new Date(contest!.from).toISOString().split('T')[0]} max={new Date(contest!.until).toISOString().split('T')[0]} onChange={(e) => { setFrom(e.target.value); setValidationError(""); }} className="form-control" />
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label className="control-label">Until</label>
                                        <input id="endDate" type="date" min={new Date(contest!.from).toISOString().split('T')[0]} max={new Date(contest!.until).toISOString().split('T')[0]} onChange={(e) => { setUntil(e.target.value); setValidationError(""); }} className="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="container">
                            <div className="row row-cols-3" id="generatedBoxes">
                                {Object.values(selectedTimesByDay).map((box, index) => (
                                    <div key={index}>
                                        {box as React.ReactNode}
                                    </div>
                                ))}</div>
                            <br />
                        </div>
                        <div className="form-group">
                            <button onClick={(e) => { CreateNewGame(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                        </div>
                    </form>
                </div >
            </div >
            <div>
                <Link href="/ContestAdmin/Contest">Back to List</Link>
            </div>
        </>
    );
}
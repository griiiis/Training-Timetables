"use client"
import { IGameType } from "@/domain/IGameType";
import { ILevel } from "@/domain/ILevel";
import { ITeam } from "@/domain/ITeam";
import { IUserContestPackage } from "@/domain/IUserContestPackage";
import GameTypeService from "@/services/GameTypeService";
import LevelService from "@/services/LevelService";
import TeamService from "@/services/TeamService";
import UserContestPackageService from "@/services/UserContestPackageService";
import Link from "next/link";
import { useParams } from "next/navigation";
import { useEffect, useState } from "react";

export default function Overview() {
    let { id } = useParams();
    
    const [teams, setTeams] = useState<ITeam[]>([]);
    const [userContestPackages, setUserContestPackages] = useState<IUserContestPackage[]>([]);
    const [gameTypes, setGameTypes] = useState<IGameType[]>([]);
    const [teachers, setTeachers] = useState<IUserContestPackage[]>([]);
    const [levels, setLevels] = useState<ILevel[]>([]);

    const [isLoading, setIsLoading] = useState(true);

    const loadData = async () => {
        const teamsResponse = await TeamService.getCurrentContestTeams(id.toString());
        const userPackagesResponse = await UserContestPackageService.getContestUsersWithoutTrainers(id.toString());
        const gameTypesResponse = await GameTypeService.getCurrentContestGameTypes(id.toString());
        const teachersResponse = await UserContestPackageService.getContestTeachers(id.toString());
        const levelsResponse = await LevelService.getCurrentContestLevels(id.toString());
        if(teamsResponse.data && userPackagesResponse.data && gameTypesResponse.data && teachersResponse.data && levelsResponse.data){
            setTeams(teamsResponse.data);
            setUserContestPackages(userPackagesResponse.data)
            setGameTypes(gameTypesResponse.data)
            setTeachers(teachersResponse.data)
            setLevels(levelsResponse.data)
            setIsLoading(false);
        }
    }

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Contest Overview - LOADING</h1>)


    return (
        <>
            <h1 className="middle">My Contest</h1>
            <hr />
            <div className="container">
                <div className="row">
                    {gameTypes.map((gameType) => (
                        <div key={gameType.id} className="col-lg-4 col-md-6 mb-4">
                            <div className="card h-100">
                                <div className="card-body">
                                    <h5 className="card-title">{gameType.gameTypeName}</h5>
                                    <p className="card-text">Teams Count: {teams.filter(g => g.gameTypeId === gameType.id).length}</p>
                                    <p className="card-text">People Count: {userContestPackages.filter(u => u.team?.gameTypeId === gameType.id).length}</p>
                                    <p className="card-text">Trainers Count: {teachers.filter(e => e.appUser.rolePreferences.some(preference => preference.gameTypeId === gameType.id)).length}</p>
                                    <hr />
                                    <h6 className="mt-4">Tasemete järgi</h6>
                                    <ul className="list-unstyled">
                                        {levels.map((level) => (
                                            <li key={level.id}>{level.title}: {userContestPackages.filter(e => e.team?.gameTypeId === gameType.id && e.team?.levelId === level.id).length}</li>
                                        ))}
                                    </ul>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
            <div>
                <Link className="middle" href="/ContestAdmin/Contest/">Back to List</Link>
            </div>
        </>
    );
};
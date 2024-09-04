"use client"
import { IGameType } from "@/domain/IGameType";
import { ILevel } from "@/domain/ILevel";
import { IRolePreference } from "@/domain/IRolePreference";
import GameTypeService from "@/services/GameTypeService";
import LevelService from "@/services/LevelService";
import RolePreferenceService from "@/services/RolePreferenceService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import React, { useEffect, useState } from "react";


export default function RolePreference() {
    let { id } = useParams();
    const router = useRouter();
    const [isLoading, setIsLoading] = useState(true);
    const [gameTypes, setGameTypes] = useState<IGameType[]>([]);
    const [levelSelectList, setLevelSelectList] = useState<ILevel[]>([]);
    const [selectedLevelsList, setSelectedLevelsList] = useState(Array(Array("")));
    const [previousRolePreferences, setPreviousRolePreferences] = useState<IRolePreference[]>([]);
    const [validationError, setValidationError] = useState("");

    const editRolePreferences = async () => {
        const editRolePreferences = {
            selectedLevelsList: selectedLevelsList,
            ContestId : id
        }

        const response = await RolePreferenceService.addRolePreferences(editRolePreferences);
        if (response.data) {
            router.push("/MyContests");
        }
        if (response.errors && response.errors.length > 0) {
            setValidationError(response.errors[0]);
        }

    };

    const loadData = async () => {
        const gameTypesResponse = await GameTypeService.getCurrentContestGameTypes(id.toString());
        const levelsResponse = await LevelService.getCurrentContestLevels(id.toString());
        const rolePrefererencesResponse = await RolePreferenceService.getAll();
        if (gameTypesResponse.data && levelsResponse.data && rolePrefererencesResponse.data) {
            var i = 0
            for (const gameType of gameTypesResponse.data) {
                var idsList = new Array("");
                for (const level of levelsResponse.data) {
                    if (rolePrefererencesResponse.data && rolePrefererencesResponse.data.some(p => p.levelId === level.id && p.gameTypeId === gameType.id)){
                        idsList.push(level.id)
                    }
                }
                selectedLevelsList[i++] = idsList;
            }
            setPreviousRolePreferences(rolePrefererencesResponse.data);
            setLevelSelectList(levelsResponse.data);
            setGameTypes(gameTypesResponse.data);
            setIsLoading(false);
        }
    }
    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>RolePreferences - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Trainer Preferences</h1>
            <hr />
            <form>
                <div className="container">
                    <div className={`row`}>
                        {gameTypes.map((gameType, index) => (
                            <div key={index} className="col-md-4">
                                <h2>{gameType.gameTypeName}</h2>
                                <div className="form-group">
                                    {previousRolePreferences ? (
                                        <>
                                            <select multiple className="form-control" required onChange={(e) => {
                                                const selectedLevels = Array.from(e.target.selectedOptions, option => option.value);
                                                setSelectedLevelsList(prevLevels => {
                                                    const updatedLevels = [...prevLevels];
                                                    updatedLevels[index] = selectedLevels;
                                                    return updatedLevels;
                                                });

                                            }}>
                                                <option value="-1">None</option>
                                                {levelSelectList.map((level) => (
                                                    <option key={level.id} value={level.id} selected={previousRolePreferences && previousRolePreferences.some(p => p.levelId === level.id && p.gameTypeId === gameType.id)}>
                                                        {level.title}
                                                    </option>
                                                ))}
                                            </select>
                                        </>
                                    ) : (
                                        <>
                                            <select multiple className="form-control" onChange={(e) => {
                                                const selectedLevels = Array.from(e.target.selectedOptions, option => option.value);

                                                setSelectedLevelsList(prevLevels => {
                                                    const updatedLevels = [...prevLevels];
                                                    updatedLevels[index] = selectedLevels;
                                                    return updatedLevels;
                                                });
                                            }}
                                            
                                            ><option value="-1">None</option>
                                                {levelSelectList.map((level) => {
                                                return (
                                                    <option key={level.id} value={level.id}>
                                                        {level.title}
                                                    </option>
                                                )
                                            })}</select>
                                        </>
                                    )}

                                </div>
                            </div>
                        ))}
                        <div className="middle form-group">
                            <br />
                            <br />
                            <button onClick={(e) => { editRolePreferences(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                        </div>
                    </div>
                </div>
            </form>
            <div>
                <Link className="middle" href="/Contests/MyContests">Back To List</Link>
            </div>
        </>
    );
}

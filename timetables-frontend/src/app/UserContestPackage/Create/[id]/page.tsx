"use client"
import { IContest } from "@/domain/IContest";
import { ILevel } from "@/domain/ILevel";
import { IPackageGameTypeTime } from "@/domain/IPackageGameTypeTime";
import ContestService from "@/services/ContestService";
import LevelService from "@/services/LevelService";
import PackageGameTypeTimeService from "@/services/PackageGameTypeTimeService";
import UserContestPackageService from "@/services/UserContestPackageService";
import { IUserInfo } from "@/state/AppContext";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Create() {
    let { id } = useParams();
    const router = useRouter();
    const [contest, setContest] = useState<IContest>();
    const [packages, setPackages] = useState<IPackageGameTypeTime[]>([]);
    const [levels, setLevels] = useState<ILevel[]>([]);
    const [selectedPackageId, setSelectedPackageId] = useState("");
    const [levelId, setLevelId] = useState("");
    const [isLoading, setIsLoading] = useState(true);

    const CreateNewUserPackage = async () => {
        const UserContestPackageData = {
            packageGameTypeTimeId: selectedPackageId,
            hoursAvailable: contest!.totalHours,
            contestId: contest!.id,
            levelId: levelId
        }
        const response = await UserContestPackageService.postUserContestPackage(UserContestPackageData);
        if (response.data) {
            let userInfo: IUserInfo = JSON.parse(localStorage.getItem("userInfo")!);
            userInfo.role = "Participant"
            localStorage.setItem("userInfo", JSON.stringify(userInfo));
            window.dispatchEvent(new Event("storage"));
            router.push("/MyContests");
        }
    };

    
    const loadData = async () => {
        const contestResponse = await ContestService.getContestInformation(id.toString());
        const packagesResponse = await PackageGameTypeTimeService.getCurrentContestPackages(id.toString());
        const levelsResponse = await LevelService.getCurrentContestLevels(id.toString());
        if (contestResponse.data && packagesResponse.data && levelsResponse.data) {
            setContest(contestResponse.data);
            setPackages(packagesResponse.data);
            setLevels(levelsResponse.data);
            setIsLoading(false);
        };
    }
    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Join the Contest - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Join the Contest</h1>
            <hr />
            <br />
            <form>
                <div className="mb-3 row">
                    <div className="col-md-4">
                        <div className="form-group">
                            <label className="control-label" htmlFor="Packages">Packages</label>
                            <select className="form-control" onChange={(e) => setSelectedPackageId(e.target.value)}><option>Please choose one option</option>{packages.map((packages) => {
                                return (
                                    <option key={packages.id} value={packages.id}>
                                        {packages.packageGtName}
                                    </option>
                                );
                            })}</select>
                        </div>
                        <div className="form-group">
                            <label className="control-label" htmlFor="Levels">Levels</label>
                            <select className="form-control" onChange={(e) => setLevelId(e.target.value)}><option>Please choose one option</option>{levels.map((level) => {
                                return (
                                    <option key={level.id} value={level.id}>
                                        {level.title}
                                    </option>
                                );
                            })}</select>
                        </div>
                        <div className="form-group">
                            <button onClick={(e) => { CreateNewUserPackage(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                        </div>

                    </div>
                </div>
            </form>

            <div>
                <Link href="/ContestAdmin/Court">Back to List</Link>
            </div>
        </>
    );
}
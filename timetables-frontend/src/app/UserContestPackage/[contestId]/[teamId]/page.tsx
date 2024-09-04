"use client"
import { IUserContestPackage } from "@/domain/IUserContestPackage";
import { IAppUser } from "@/domain/Identity/IAppUser";
import AppUserService from "@/services/AppUserService";
import UserContestPackageService from "@/services/UserContestPackageService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import React, { use, useEffect, useState } from "react";

export default function UserTeam() {
    let { contestId, teamId } = useParams();
    const router = useRouter();
    const [isLoading, setIsLoading] = useState(true);
    const [userEmail, setUserEmail] = useState("");
    const [otherUsers, setOtherUsers] = useState<IAppUser[]>([]);

    const AddToTeam = async () => {
        const data = {
            userEmail : userEmail
        }
        const response = await UserContestPackageService.AddToTeam(teamId.toString(), data);
        if (response.data) {
            router.push("/MyContests");
        }
    }

    const loadData = async () => {
        const otherContestUsers = await AppUserService.getContestUsersToAddToTeam(contestId.toString(), teamId.toString())
        if (otherContestUsers.data) {
            setOtherUsers(otherContestUsers.data);
            setIsLoading(false);
        }
    }
    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Add People To Your Team - LOADING</h1>)

    return (
        <>
            <h1 className="middle">Add People To Your Team</h1>
            <hr />
            <div className="row">
                <div className="col-md-4">
                    <div className="form-group">
                        <label className="control-label" htmlFor="User">List of Contest Users</label>
                        <select className="form-control" onChange={(e) => setUserEmail(e.target.value)}><option>Please select one!</option>{otherUsers.map((user) => {
                            return (
                                <option key={user.email} value={user.email}>
                                    {`${user.firstName} ${user.lastName}`}
                                </option>
                            );
                        })}</select>
                    </div>
                    <div className="form-group">
                        <button onClick={(e) => { AddToTeam(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                    </div>
                </div>
            </div>
            <div>
                <Link href="/MyContests">Back to List</Link>
            </div>
        </>
    );
}

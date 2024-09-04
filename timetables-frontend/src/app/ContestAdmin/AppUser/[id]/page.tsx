"use client"
import { IUserContestPackage } from "@/domain/IUserContestPackage";
import UserContestPackageService from "@/services/UserContestPackageService";
import Link from "next/link";
import { useParams } from "next/navigation";
import {useEffect, useState } from "react";

export default function Index() {

    let {id} = useParams();
    const[userContestPackages, setUserContestPackages] = useState<IUserContestPackage[]>([])
    const [isLoading, setIsLoading] = useState(true);

    const loadData = async () => {
        const userContestPackagesResponse = await UserContestPackageService.getContestAllUsers(id.toString());

        if(userContestPackagesResponse.data){
            setUserContestPackages(userContestPackagesResponse.data);
            setIsLoading(false);
        }
    };




    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Participants and Trainers  - LOADING</h1>)


    return (
        <div>
            <h1 className="middle">Participants and Trainers</h1>
            <table className="table">
                <thead>
                    <tr>
                        <th>First Name</th>
                        <th>Last Name</th>
                        <th>Email</th>
                        <th>Role</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {userContestPackages.map(item => (
                        <tr key={item.id}>
                            <td>{item.appUser.firstName}</td>
                            <td>{item.appUser.lastName}</td>
                            <td>{item.appUser.email}</td>
                            <td></td>
                            <td>
                                <Link href={`/ContestAdmin/AppUser/Edit/${item.appUserId}`}>Edit</Link>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );

}
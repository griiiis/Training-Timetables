"use client"
import { IAppRole } from "@/domain/Identity/IAppRole";
import { IAppUser } from "@/domain/Identity/IAppUser";
import { IAppUserModel } from "@/domain/Models/IAppUserModel";
import AppUserService from "@/services/AppUserService";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Edit(){
    let {id} = useParams();
    const router = useRouter();
    const[user, setUser] = useState<IAppUser>();
    const[roles, setRoles] = useState<IAppRole[]>();
    const[roleId, setRoleId] = useState("");
    const [isLoading, setIsLoading] = useState(true);

    const editUserRole = async () => {
        const userRoleData : IAppUserModel = {
            appUser: user!,
            roleSelectList: [],
            selectedRoleId: roleId
        };

        const response = await AppUserService.putUser(id.toString(), userRoleData);
        if (response.data) {
            router.push("/ContestAdmin/Contest");
        }
    }
    

    const loadData = async () => {
        const appUserResponse = await AppUserService.getUser(id.toString());
        if(appUserResponse.data){
            setUser(appUserResponse.data.appUser);
            setRoles(appUserResponse.data.roleSelectList)
            setRoleId(appUserResponse.data.selectedRoleId)
            setIsLoading(false);
        }
    };

    useEffect(() => { loadData() }, []);

    if (isLoading) return (<h1>Edit User Role - LOADING</h1>)


     
return (
    <div>
        <h1 className="middle">Edit User Role</h1>
        <hr />
        <div className="row">
            <div className="col-md-4">
                <form>
                    <div className="form-group">
                        <label className="control-label">First Name</label>
                        <input readOnly value={user!.firstName} className="form-control" />
                    </div>
                    <div className="form-group">
                        <label className="control-label">Last Name</label>
                        <input readOnly value={user!.lastName} className="form-control" />
                    </div>
                    <input type="hidden" value={user!.id} />
                    <div className="form-group">
                        <label className="control-label">Username</label>
                        <input readOnly value={user!.firstName} className="form-control" />
                    </div>
                    <div className="form-group">
                        <label className="control-label">Role</label>
                        <select value={roleId} onChange={(e) => setRoleId(e.target.value)} className="form-control">
                            {roles!.map(role => (
                                <option key={role.id} value={role.id}>{role.name}</option>
                            ))}
                        </select>
                    </div>
                    <div className="form-group">
                        <button onClick={(e) => { editUserRole(), e.preventDefault(); }} type="submit" className="btn btn-primary">Submit</button>
                    </div>
                </form>
            </div>
        </div>
        <div>
            <Link href="/ContestAdmin/Contest/">Back to List</Link>
        </div>
    </div>
);


}
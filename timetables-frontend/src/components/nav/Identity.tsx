// src/components/Identity.js
"use client"
import AccountService from "@/services/AccountService";
import { IUserInfo } from "@/state/AppContext";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useState, useEffect } from "react";

export default function Identity() {
    const [user, setUser] = useState<IUserInfo>();

    useEffect(() => {
        const handleStorageChange = () => {
            const storedUser = localStorage.getItem("userInfo");
            setUser(storedUser ? JSON.parse(storedUser) : null);
        };

        handleStorageChange();

        window.addEventListener("storage", handleStorageChange);

        return () => {
            window.removeEventListener("storage", handleStorageChange);
        };
    }, []);

    return user ? <LoggedIn user={user} /> : <LoggedOut />;
}

const LoggedIn = ({ user} : { user: IUserInfo} ) => {
    const router = useRouter();
    const doLogout = async () => {
        const logOutData = {
            refreshToken: user.refreshToken
        };

        await AccountService.logout(logOutData);
        localStorage.removeItem("userInfo");
        router.push("/");
        window.dispatchEvent(new Event("storage"));
    };

    return (
        <li className="text-end d-flex align-items-center">
            <Link href="/" className="btn btn-outline-light me-2" title="Manage">Hello, {user.firstName} {user.lastName}</Link>
            <button onClick={doLogout} className="btn btn-outline-light me-2" title="Logout">Logout</button>
        </li>
    );
};

const LoggedOut = () => {
    return (
        <div className="text-end">
            <Link href="/Account/Login" className="btn btn-outline-light me-2">Login</Link>
            <Link href="/Account/Register" className="btn btn-warning">Register</Link>
        </div>
    );
};

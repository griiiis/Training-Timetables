"use client"
import Link from "next/link";
import Identity from "./Identity";
import { IUserInfo } from "@/state/AppContext";
import { useEffect, useState } from "react";

export default function Header() {

    const [userInfo, setUserInfo] = useState<IUserInfo | null>(null);

    useEffect(() => {
        const handleStorageChange = () => {
            const json = localStorage.getItem("userInfo");
            if (json) {
                const user = JSON.parse(json);
                setUserInfo(user);
            } else {
                setUserInfo(null);
            }
        };
        handleStorageChange();
        window.addEventListener("storage", handleStorageChange);
        return () => {
            window.removeEventListener("storage", handleStorageChange);
        };
    }, []);


    if (userInfo && userInfo.role === "Contest Admin") {
        return (
            <>
                <header className="p-3 bg-dark text-white">
                    <div className="container d-flex justify-content-md-between">
                        <Link href="/" className="d-flex align-items-center text-white text-decoration-none">TimeTables</Link>
                        <ul className="nav center-anchor">
                            <li>
                                <Link href="/" className="nav-link px-2 text-secondary">Home</Link>
                            </li>
                            <li>
                                <Link href="/ContestAdmin/Contest" className="nav-link px-2 text-white">My Contests</Link>
                            </li>
                            <li>
                                <a className="nav-link px-2 text-white dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                    Settings
                                </a>
                                <ul className="dropdown-menu">
                                    <li>
                                        <Link href="/ContestAdmin/ContestType" className="dropdown-item nav-link text-dark">ContestTypes</Link>
                                    </li>
                                    <li>
                                        <Link href="/ContestAdmin/GameType" className="dropdown-item nav-link text-dark">GameTypes</Link>
                                    </li>
                                    <li>
                                        <Link href="/ContestAdmin/Court" className="nav-link text-dark">Courts</Link>
                                    </li>
                                    <li>
                                        <Link href="/ContestAdmin/Level" className="nav-link text-dark">Levels</Link>
                                    </li>
                                    <li>
                                        <Link href="/ContestAdmin/Location" className="nav-link text-dark">Locations</Link>
                                    </li>
                                    <li  >
                                        <Link href="/ContestAdmin/PackageGameTypeTime" className="nav-link text-dark">Packages</Link>
                                    </li>
                                    <li>
                                        <Link href="/ContestAdmin/TimeOfDay" className="nav-link text-dark">TimeOfDays</Link>
                                    </li>
                                    <li>
                                        <Link href="/ContestAdmin/Time" className="nav-link text-dark">Times</Link>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                        <Identity />
                    </div>
                </header>
                <div className="b-example-divider"></div>
            </>
        );
    } else if (userInfo && (userInfo.role === "Participant")) {
        return (
            <>
                <header className="p-3 bg-dark text-white">
                    <div className="container d-flex justify-content-md-between">
                        <Link href="/" className="d-flex align-items-center text-white text-decoration-none">TimeTables</Link>
                        <ul className="nav">
                            <li>
                                <Link href="/" className="nav-link px-2 text-secondary">Home</Link>
                            </li>
                            <li>
                                <Link href="/MyContests" className="nav-link px-2 text-white">My Contests</Link>
                            </li>
                        </ul>
                        <Identity />
                    </div>
                </header>
                <div className="b-example-divider"></div>
            </>
        );
    } else {

        return (
            <>
                <header className="p-3 bg-dark text-white">
                    <div className="container d-flex justify-content-md-between">
                        <Link href="/" className="d-flex align-items-center text-white text-decoration-none">TimeTables</Link>
                        <ul className="nav">
                            <li>
                                <Link href="/" className="nav-link px-2 text-secondary">Home</Link>
                            </li>
                        </ul>
                        <Identity />
                    </div>
                </header>
                <div className="b-example-divider"></div>
            </>
        )
    }
}
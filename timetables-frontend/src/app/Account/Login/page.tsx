// src/components/Login.js
"use client"
import AccountService from "@/services/AccountService";
import { useRouter } from "next/navigation";
import { useState, useEffect } from "react";

export default function Login() {
    const router = useRouter();
    const [email, setEmail] = useState("admin@eesti.ee");
    const [pwd, setPwd] = useState("Mina!1");
    const [validationError, setValidationError] = useState("");

    const validateAndLogin = async () => {
        const loginData = {
            email: email,
            password: pwd
        };
        const response = await AccountService.login(loginData);
        if (response.data) {
            response.data.expiresIn = Date.now() + response.data.expiresIn * 1000;
            localStorage.setItem('userInfo', JSON.stringify(response.data));
            window.dispatchEvent(new Event("storage"));
            router.push("/");
        } else if (response.errors && response.errors.length > 0) {
            setValidationError(response.errors[0]);
        }
    };

    useEffect(() => {
        const userInfo = localStorage.getItem("userInfo");
        if (userInfo !== null) {
            router.push("/");
        }
    }, []);

    return (
        <div className="row">
            <div className="col-md-5">
                <h2>Log in</h2>
                <hr />
                <div className="text-danger" role="alert">{validationError}</div>
                <div className="form-floating mb-3">
                    <input value={email}
                        onChange={(e) => { setEmail(e.target.value); setValidationError(""); }}
                        id="email" type="email" className="form-control" autoComplete="email" placeholder="name@example.com"></input>
                    <label htmlFor="email" className="form-label">Email</label>
                </div>
                <div className="form-floating mb-3">
                    <input value={pwd}
                        onChange={(e) => { setPwd(e.target.value); setValidationError(""); }}
                        id="password" type="password" className="form-control" autoComplete="password" placeholder="password"></input>
                    <label htmlFor="password" className="form-label">Password</label>
                </div>
                <div>
                    <button onClick={validateAndLogin} className="w-100 btn btn-lg btn-primary">Log in</button>
                </div>
            </div>
        </div>
    );
}

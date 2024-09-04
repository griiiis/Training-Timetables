"use client"
import AccountService from "@/services/AccountService";
import { useRouter } from "next/navigation";
import { useState } from "react";


export default function Register() {
    const router = useRouter();
    const [firstName, setFirstName] = useState("mina");
    const [lastName, setLastName] = useState("mina");
    const [gender, setGender] = useState("0");
    const [age, setAge] = useState(0);
    const [email, setEmail] = useState("uus@gmail.com");
    const [password, setPassword] = useState("Uus!12");
    const [confirmPassword, setconfirmPassword] = useState("Uus!12");

    enum EGender {
        Male = "0",
        Female = "1"
    }

    const [validationError, setValidationError] = useState("");

    const validateAndRegister = async () => {
        if(email.length < 5 || password.length < 6){
            setValidationError("Invalid input lengths!");
            return;
        } else if(password !== confirmPassword){
            setValidationError("Passwords don't match!");
            return;
        } else if(parseInt(gender) === -1 ||  age === 0){
            setValidationError("Invalid inputs!");
            return;
        }

        const registrationData = {
            email: email, 
            password: password,
            firstName: firstName,
            lastName: lastName,
            age: age,
            gender: parseInt(gender)
        }

        const response = await AccountService.register(registrationData);
        if (response.data){
            response.data.expiresIn = Date.now() + response.data.expiresIn * 1000;
            localStorage.setItem('userInfo', JSON.stringify(response.data));
            window.dispatchEvent(new Event("storage"));
            router.push("/");
        }
    
        if (response.errors && response.errors.length > 0){
            setValidationError(response.errors[0]);
        }
    }


    return (
        <>
        <main className="pb-3">
        
        <h1>Register</h1>
        
        <div className="row">
            <div className="col-md-4">
                    <h2>Create a new account.</h2>
                    <hr />
                    <div className="text-danger" role="alert">{validationError}</div>
                    <div className="form-floating mb-3">
                    <input value={firstName}
                            onChange={(e) =>{setFirstName(e.target.value); setValidationError("");}}
                            id="firstName" type="text" className="form-control" autoComplete="firstName" placeholder="First Name"></input>
                        <label htmlFor="Input_FirstName" className="form-label">First name</label>
                    </div>
                    <div className="form-floating mb-3">
                    <input value={lastName}
                            onChange={(e) =>{setLastName(e.target.value); setValidationError("");}}
                            id="lastName" type="text" className="form-control" autoComplete="lastName" placeholder="Last Name"></input>
                        <label htmlFor="Input_LastName" className="form-label">Last name</label>
                    </div>
                    <div className="form-group mb-2">
                    <select className="form-control" onChange={(e) => setGender(e.target.value)}><option value={-1}>Please choose one option</option>{Object.keys(EGender).map((key, index) => {
                                return (
                                    <option key={index} value={index}>
                                        {key}
                                    </option>
                                );
                            })}
                        </select>
                    </div>
                    <div className="form-floating mb-3">
                    <input value={age}
                            onChange={(e) =>{setAge(e.target.valueAsNumber); setValidationError("");}}
                            id="age" type="number" className="form-control" autoComplete="age" placeholder="Age" min="0"></input>
                        <label htmlFor="Input_Age" className="form-label">Age</label>
                    </div>
                    
                    <div className="form-floating mb-3">
                    <input value={email}
                            onChange={(e) =>{setEmail(e.target.value); setValidationError("");}}
                            id="email" type="text" className="form-control" autoComplete="email" placeholder="Email"></input>
                        <label htmlFor="Input_Email" className="form-label">Email</label>
                    </div>
                    <div className="form-floating mb-3">
                        <input value={password}
                            onChange={(e) =>{setPassword(e.target.value); setValidationError("");}}
                            id="password" type="password" className="form-control" autoComplete="password" placeholder="Password"></input>
                        <label htmlFor="Input_Password" className="form-label">Password</label>
                    </div>
                    <div className="form-floating mb-3">
                    <input value={confirmPassword}
                            onChange={(e) =>{setconfirmPassword(e.target.value); setValidationError("");}}
                            id="confirmPassword" type="password" className="form-control" autoComplete="confirmPassword" placeholder="Confirm Password"></input>
                        <label htmlFor="Input_ConfirmPassword" className="form-label">Confirm Password</label>
                    </div>
                    <div>
                        <button onClick={(e) => validateAndRegister()} className="w-100 btn btn-lg btn-primary">Register</button>
                    </div>

            </div>
        </div>
        
        
            </main>
        </>
    );
}

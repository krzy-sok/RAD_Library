import React, { useState, createContext, useContext } from 'react';
import { Header, Footer } from './shared/_Layout';
import { useForm, FormProvider, useFormContext } from "react-hook-form";
import { InputField } from "./shared/InputField"

//type AuthContext = {
//    role?: string | null
//    username?: string | null
//};

//const authContext = createContext<AuthContext | undefined>(undefined)

//type AuthProviderProps = PropsWithChildren


export const LoginForm = () => {
    const methods = useForm();
    const [feedback, setFeedback] = useState<JSX.Element>(<div></div>)

    const onSubmit = methods.handleSubmit(data => {
        console.log(data)
        //make put request to /user

       LoginWrapper(data);
    })
    return (
        <div>
            <Header />
            <div>
                {feedback}
                <div className="row" >
                    <div className="col-md-4" >
                        <FormProvider {...methods}>
                            <form onSubmit={e => e.preventDefault()} noValidate>
                                <InputField label="Username or Email" type="text" id="userNameOrEmail" defaultVal="username" />
                                <InputField label="Password" type="password" id="password" defaultVal="password" />

                                <div className="form-group" >
                                    <button onClick={onSubmit} className="btn btn-primary">
                                        Log in
                                    </button>
                                </div>
                            </form>
                        </FormProvider>
                    </div>
                </div>
            </div >
            <Footer />
        </div>
    );
    async function LoginWrapper(data) {
        const result = await SendLoginRequest(data)
        if (result == 200) {
            setFeedback(<div style={{ color: "green" }} > Login succesful login</div>);
        }
        else if (result == 406) {
            setFeedback(<div style={{ color: "red" }} >Incorrect login data</div>)
        }
        else {
            setFeedback(<div style={{ color: "red" }} >Bad request</div>)
        }
    }

}

async function SendLoginRequest(data) {
    console.log("\n********\n\n in send request \n\n ******\n");
    console.log(data)
    const requestOptions = {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    };
    const response = await fetch('/user/login', requestOptions);
    if (response.ok) {
        return 200;
    }
    else if (response.status == 406) {
        return 406;
    }
    else {
        return 400;
    }
}

//export default function AuthProvider({ children }: AuthProviderProps) {
//    const [role, setRole] = useState<string | undefined>();
//    const [username, setUsername] = useState<string | undefined>();

//    return <authContext.Provider value={ }>{children}</authContext.Provider>
//}

//export function useAuth() {
//    const context = useContext(authContext);

//    if (context === undefined) {
//        throw new Error("Use auth must be used inside of auth provider");
//    }

//    return context;
//}
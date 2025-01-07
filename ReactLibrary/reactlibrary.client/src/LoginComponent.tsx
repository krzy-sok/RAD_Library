import { useState } from 'react';
import { Header, Footer } from './shared/_Layout';
import { useForm, FormProvider } from "react-hook-form";
import { InputField } from "./shared/InputField"
import { useAuth } from "./shared/AuthProvider"
import { Navigate } from 'react-router-dom';



export const LoginForm = () => {
    const methods = useForm();
    const [feedback, setFeedback] = useState<JSX.Element>(<div></div>)
    const { handleLogin, username } = useAuth();

    const onSubmit = methods.handleSubmit(data => {
        console.log(data)
        //make put request to /user

        LoginWrapper(data);
        return <Navigate to="/catalogue" />
    })
    return (username ? <Navigate to="/catalogue" /> :
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
        const result = await handleLogin(data)
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


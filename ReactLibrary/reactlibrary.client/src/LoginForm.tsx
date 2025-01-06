import React, { useState } from 'react';
import { Header, Footer } from './shared/_Layout';
import { useForm, FormProvider, useFormContext } from "react-hook-form";
import { InputField } from "./shared/InputField"

export const LoginForm = () => {
    const methods = useForm();
    const [feedback, setFeedback] = useState<JSX.Element>(<div></div>)

    const onSubmit = methods.handleSubmit(data => {
        console.log(data)
        //make put request to /user
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
}
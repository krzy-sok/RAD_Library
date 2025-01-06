import React, { useState } from 'react';
import { Header, Footer } from './shared/_Layout';
import { useForm, FormProvider, useFormContext } from "react-hook-form";
import { InputField } from "./shared/InputField"

export const RegisterForm = () => {
    const methods = useForm();
    const [feedback, setFeedback] = useState<JSX.Element>(<div></div>)

    const onSubmit = methods.handleSubmit(data => {
        console.log(data)
        if (data.password != data.confirmPassword) {
            setFeedback(<div style={{ color: "red" }} >Passwords do not mach </div>)
        }
        else if (!(data.phoneNumber.match('[0-9]{9}'))) {
            setFeedback(<div style={{ color: "red" }} >Incorrect phone number </div>)
        }
        //make post request to /user/register
        else{
            SendRegisterRequest(data);
        }
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
                                <InputField label="Username" type="text" id="userName" defaultVal="" />
                                <InputField label="First Name" type="text" id="firstName" defaultVal="" />
                                <InputField label="Last Name" type="text" id="lastName" defaultVal="" />
                                <InputField label="Email address" type="email" id="email" defaultVal="" />
                                <InputField label="Phone Number" type="text" id="phoneNumber" defaultVal="" />
                                <InputField label="Password" type="password" id="password" defaultVal="" />
                                <InputField label="Confirm Password" type="password" id="confirmPassword" defaultVal="" />

                                <div className="form-group" >
                                    <button onClick={onSubmit} className="btn btn-primary">
                                        Register
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

    async function SendRegisterRequest(data) {
        console.log("\n********\n\n in send request \n\n ******\n");
        console.log(data)
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        };
        const response = await fetch('/user/register', requestOptions);
        if (response.ok) {
            setFeedback(<div style={{ color: "green" }} >Registration succesfull, please login</div>);
        }
        else if (response.status == 406) {
            setFeedback(<div style={{ color: "red" }} >User with that email or username alredy exists</div>)
        }
        else {
            setFeedback(<div style={{ color: "red" }} >Invalid form data</div>)
        }
    }
}
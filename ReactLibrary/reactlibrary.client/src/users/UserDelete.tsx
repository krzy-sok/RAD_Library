import { useState } from 'react';
import { InputField } from "../shared/InputField";
import { Link, Navigate } from 'react-router-dom';
import { useForm, FormProvider } from "react-hook-form";
import { Header, Footer } from '../shared/_Layout';
import { useAuth } from '../shared/AuthProvider';   

export const DeleteUserForm = () => {
    const [feedback, setFeedback] = useState<JSX.Element>(<div></div>)
    const { handleLogout } = useAuth();

    const methods = useForm();
    const onSubmit = methods.handleSubmit(data => {
        //console.log(data)
        makeDELETErequest(data);
    })


    return (
        <div>
            <h4> Are you sure you want to your account?</h4>
            < hr />
            {feedback}
            <div className="row" >
                <div className="col-md-4" >
                    <FormProvider {...methods}>
                        <form onSubmit={e => e.preventDefault()} noValidate>
                            <InputField label="Email" type="email" id="userEmail" defaultVal="" />
                            <InputField label="Password" type="password" id="password" defaultVal="" />

                            <div className="form-group" >
                                <button onClick={onSubmit} className="btn btn-danger">
                                    Delete
                                </button>
                            </div>
                        </form>
                    </FormProvider>
                    <div>
                        <Link to="/user" > Return </Link>
                    </div>
                </div>
            </div>
        </div>
    );
    async function makeDELETErequest(data) {
        console.log(data)
        //call to api /book/bookID with PUT method
        console.log("\n*******\n\n delete request \n\n *********\n")
        const requestOptions = {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        };
        const response = await fetch('/user/delete', requestOptions);
        if (response.status == 403) {
            setFeedback(<div style={{ color: "red" }} >Cannot delete user with leases</div>);
        }
        else if (response.status == 401) {
            setFeedback(<div style={{ color: "red" }} >Incorrect email or password</div>);
        }
        else if (response.ok) {
            setFeedback(<div style={{ color: "green" }} >User deleted successfully</div>);
            handleLogout()
        }
    }
}


export const DeleteUser = () => {
    const header = Header();
    const footer = Footer();
    const { username, handleLogout } = useAuth();
    return (username ?
        <div>
            {header}
            <h1>Edit </h1>
            {DeleteUserForm()}
            {footer}
        </div>
        : <Navigate to="/catalogue" />
    )
} 
import React, { useEffect, useState } from 'react';
import { Header, Footer } from '../shared/_Layout'
import { useParams } from 'react-router-dom';
import { Lease } from "./LeasesList.tsx"
import { useForm, FormProvider } from "react-hook-form";
import { InputField } from "../shared/InputField";
import { Link, Navigate } from 'react-router-dom';
import { useAuth } from '../shared/AuthProvider';    


export const EditLeaseForm = (leaseId:string) => {
    const [lease, setLease] = useState<Lease>()
    const [feedback, setFeedback] = useState<JSX.Element>(<div></div>)
    useEffect(() => {
        getLease(parseInt(leaseId!))
    }, [leaseId]);
    const methods = useForm();

    const onSubmit =  methods.handleSubmit(data => {
        console.log(data)
        //call to api with post method
        SendChanges(data, lease)
    })
    
    return (
    lease === undefined ? <p>no lease</p> :
    <div>
        <h4> Lease</h4>
        < hr />
        { feedback}
        <div className= "row" >
            <dt className="col-sm-2">Book Title</dt>
            <dd className="col-sm-10">{lease.book.title}</dd>

            <dt className="col-sm-2">Author </dt>
            <dd className="col-sm-10">{lease.book.author}</dd>

            <dt className="col-sm-2">User</dt>
            <dd className="col-sm-10">{lease.user.userName}</dd>

            <dt className="col-sm-2">User email</dt>
            <dd className="col-sm-10">{lease.user.email}</dd>

            <dt className="col-sm-2">Start date</dt>
            <dd className="col-sm-10">{lease.leaseStart}</dd>

            <dt className="col-sm-2">Type</dt>
            <dd className="col-sm-10">{lease.type}</dd>

            <dt className="col-sm-2">version</dt>
            <dd className="col-sm-10">{lease.rowVersion}</dd>
        </div>
        <div className="row" >
            <div className="col-md-4" >
                <div>
                    <FormProvider {...methods}>
                        <form onSubmit={e => e.preventDefault()} noValidate>
                            <input type="hidden" id="id" name="id" value={lease.id} />
                            <input type="hidden" id="status" name="status" value={lease.rowVersion} />
                            {/*<input type="hidden" id="book" name="book" value={lease.book} />*/}
                            <input type="hidden" id="user" name="user" value={lease.user} />
                            <input type="hidden" id="type" name="type" value={lease.type} />
                            <input type="hidden" id="active" name="active" value={lease.active} />
                            <InputField label="End Date" type="date" id="leaseEnd" defaultVal={lease.leaseEnd} />

                            <div className="form-group" >
                                <button onClick={onSubmit} className="btn btn-primary">
                                    Save
                                </button>
                            </div>

                        </form>
                    </FormProvider>
                </div>
                {lease.type == 'Reservation' ?
                    <button onClick={() => ChangetoLease(parseInt(leaseId!), lease.rowVersion)} className="btn btn-primary">
                        Change to lease
                    </button>
                    :
                    <button onClick={() => ReturnBook(parseInt(leaseId!))} className="btn btn-primary">
                        Return Book
                    </button>
                }
            <br/>
            <div>
                <Link to="/book-leases" > Back to List </Link>
            </div>
            </div>
        </div>
    </div>
    );

    async function getLease(leaseId: number) {
        const response = await fetch('/leases/details/' + leaseId);
        //console.log(`\n************\n ${response.body} \n ***************8`)
        if (response.ok) {
            const data = await response.json();
            console.log(data)
            setLease(data);
        }
    }
    async function ChangetoLease(leaseId: number, version: string) {
        console.log(leaseId)
        console.log(version)
        const response = await fetch('/leases/lease/' + leaseId + "/" + version);
        responseBasedFeedback(response)
    }
    async function ReturnBook(leaseId: number) {
        console.log(leaseId)
        const response = await fetch('/leases/return/' + leaseId);
        //console.log(`\n************\n ${response.body} \n ***************8`)
        responseBasedFeedback(response)
    }

    function responseBasedFeedback(response) {
        console.log(response)
        if (response.status == 501) {
            setFeedback(<div style={{ color: "red" }} >Database does not exist!</div>)
        }
        else if (response.status == 409) {
            setFeedback(<div style={{ color: "red" }} > DB concurrency event! Refresh page and try again</div>)
        }
        else if (response.status == 200 ) {
            setFeedback(<div style={{ color: "green" }} > Action successful </div>)
        }
        else if (response.status == 400) {
            setFeedback(<div style={{ color: "red" }} > Id id null</div>)
        }
    }

    async function SendChanges(data, lease : Lease) {
        console.log(data)
        lease.leaseEnd = data.leaseEnd
        console.log(lease)
        //call to api /lease/leaseID with post method
        const requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(lease)
        };
        const response = await fetch('/leases/' + leaseId, requestOptions);
        console.log(response.status)
        if (response.status == 200) {
            setFeedback(<div style={{ color: "green" }} >Lease edited</div>);
        }
        else if (response.status == 409) {
            setFeedback(<div style={{ color: "red" }} > DB concurrency event! Refresh page and try again</div>)
        }
        else {
            setFeedback(<div style={{ color: "red" }} >Unexpected error while adding lease</div>);
        }
    }

};




export const LeaseEdit = () => {
    const { leaseId } = useParams()
    const header = Header();
    const footer = Footer();
    const { isadmin } = useAuth();
    return ( isadmin?
        <div>
            {header}
            <h1>Edit </h1>
            {EditLeaseForm(leaseId!)}
            {footer}
        </div>
        : <Navigate to="/catalogue" />
    )

}

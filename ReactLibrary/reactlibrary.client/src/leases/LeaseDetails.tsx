import { useEffect, useState } from 'react';
import { Header, Footer } from '../shared/_Layout.tsx'
import { useParams } from 'react-router-dom';
import { Lease } from "./LeasesList.tsx"
import { useAuth } from '../shared/AuthProvider.tsx';
import { Link, Navigate} from 'react-router-dom';



export const LeaseDetailsBlock = (leaseId: string) => {
    const [lease, setLease] = useState<Lease>()

    console.log("lease id pre parse:"+ leaseId)

    useEffect(() => {
        getLease(parseInt(leaseId!))
    }, [leaseId]);

    return (
        lease === undefined ? <p>no lease</p> :
            <div>
                <h4>Lease</h4>
                <hr />
                <dl className="row">
                    <dt className="col-sm-2">Book Title</dt>
                    <dd className="col-sm-10">{lease.book.title}</dd>

                    <dt className="col-sm-2">Authot </dt>
                    <dd className="col-sm-10">{lease.book.author}</dd>

                    <dt className="col-sm-2">User</dt>
                    <dd className="col-sm-10">{lease.user.userName}</dd>

                    <dt className="col-sm-2">User email</dt>
                    <dd className="col-sm-10">{lease.user.email}</dd>

                    <dt className="col-sm-2">Start date</dt>
                    <dd className="col-sm-10">{lease.leaseStart}</dd>

                    <dt className="col-sm-2">End Date</dt>
                    <dd className="col-sm-10">{lease.leaseEnd}</dd>

                    <dt className="col-sm-2">Type</dt>
                    <dd className="col-sm-10">{lease.type}</dd>
                </dl>
                    <button onClick={() => Unreserve(parseInt(leaseId!))} className="btn btn-primary">
                        Revert reservation
                    </button>
            </div>

    );

    async function Unreserve(leaseId: number) {
        console.log(leaseId)
    }

    async function getLease(leaseId: number) {
        console.log("lease id: "+leaseId)
        const response = await fetch('/leases/details/' + leaseId);
        if (response.ok) {
            const data = await response.json();
            console.log(data)
            setLease(data);
        }
    }
}

export const LeaseDetails = () => {
    const { leaseId } = useParams()
    const { username, isadmin } = useAuth();
    //const detailsBlock = 
    const header = Header();
    const footer = Footer();
    return (username ?
    <div>
        {header}
            <div>
                <h1>Details</h1>
                {LeaseDetailsBlock(leaseId!)}
        </div>
        {footer}
        </div>
    : <Navigate to="/" />
    )
}

export const LeaseDelete = () => {
    const { leaseId } = useParams()
    const [feedback, setFeedback] = useState<JSX.Element>(<div></div>)
    //const detailsBlock = 
    const header = Header();
    const footer = Footer();
    const { isadmin } = useAuth();
    return (isadmin?
        <div>
            {header}
            <div>
                <h1>Delete</h1>
                {feedback}
                {LeaseDetailsBlock(leaseId!)}
                <div>
                    {/*Conditional rendering based on user role and authentication */}
                    {/*{user?.isAuthenticated && !user?.roles.includes('Admin') && lease?.id && (*/}
                    <button onClick={() => DeleteConfirmed(parseInt(leaseId!))} className="btn btn-danger">
                        Delete
                    </button>
                    {/*)}*/}
                    {' | '}
                    <Link to={'/catalogue'}>Back to list</Link>
                </div>
            </div>
            {footer}
        </div>
        : <Navigate to="/catalogue" />
    );

    async function DeleteConfirmed(leaseId: number) {
        console.log(`deleting lease of id {leaseID}`);
        //make api call to lease/id as http delete
        const requestOptions = {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
        };
        const response = await fetch('/lease/' + leaseId, requestOptions);
        if (response.status == 403) {
            setFeedback(<div style={{ color: "red" }} >Cannot delete leased lease</div>);
        }
        else if (response.status == 202) {
            setFeedback(<div style={{ color: "green" }} >Lease hidden</div>);
        }
        else if (response.ok) {
            setFeedback(<div style={{ color: "green" }} >Lease deleted</div>);
        }
        else {
            setFeedback(<div style={{ color: "red" }} >Lease does not exist or different error occured</div>);
        }
    }
}





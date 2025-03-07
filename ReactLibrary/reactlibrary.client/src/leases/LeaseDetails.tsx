import { useEffect, useState } from 'react';
import { Header, Footer } from '../shared/_Layout.tsx'
import { useParams } from 'react-router-dom';
import { Lease } from "./LeasesList.tsx"
import { useAuth } from '../shared/AuthProvider.tsx';
import { Link, Navigate} from 'react-router-dom';



export const LeaseDetailsBlock = (leaseId: string) => {
    const [lease, setLease] = useState<Lease>()
    const [feedback, setFeedback] = useState<JSX.Element>(<div></div>)


    useEffect(() => {
        getLease(parseInt(leaseId!))
    }, [leaseId]);

    return (
        lease === undefined ? <p>no lease</p> :
            <div>
                <h4>Lease</h4>
                { feedback}
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
                <button onClick={() => Unreserve(lease.rowVersion)} className="btn btn-primary">
                        Revert reservation
                </button>
                <Link to="/user-leases">Back to list</Link>
            </div>

    );

    async function Unreserve(version: string) {
        console.log("/user/unlease/" + leaseId + "/" + version)
        const response = await fetch("/user/unlease/" + leaseId + "/" + version);
        console.log(response.status)
        if (response.status == 200) {
            setFeedback(<div style={{ color: "green" }} >Removed reservation of { lease.book.title}</div>);
        }
        else if (response.status == 409) {
            setFeedback(<div style={{ color: "red" }} > DB concurrency event! Refresh page and try again</div>)
        }
        else {
            setFeedback(<div style={{ color: "red" }} >Unexpected error while removing reservation</div>);
        }
    }

    async function getLease(leaseId: number) {
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





//import { useEffect, useState } from 'react';
//import { Header, Footer } from '../shared/_Layout'
//import { useParams } from 'react-router-dom';
//import { Lease } from "./LeasesList.tsx"
//import { useAuth } from '../shared/AuthProvider';
//import { Link, Navigate} from 'react-router-dom';



//export const LeaseDetailsBlock = (leaseId: string) => {
//    const [lease, setLease] = useState<Lease>()

//    useEffect(() => {
//        getLease(parseInt(leaseId!))
//    }, [leaseId]);

//    console.log(lease)
//    //const details = lease === undefined ?
//    //    <p>no lease</p>
//    //    :
//    return (
//        lease === undefined ? <p>no lease</p> :
//            <div>
//                <h4>Lease</h4>
//                <hr />
//                <dl className="row">
//                    <dt className="col-sm-2">Title</dt>
//                    <dd className="col-sm-10">{lease.title}</dd>

//                    <dt className="col-sm-2">Author</dt>
//                    <dd className="col-sm-10">{lease.author}</dd>

//                    <dt className="col-sm-2">Publisher</dt>
//                    <dd className="col-sm-10">{lease.publisher}</dd>

//                    <dt className="col-sm-2">Publication Date</dt>
//                    <dd className="col-sm-10">{lease.publicationDate}</dd>

//                    <dt className="col-sm-2">Price</dt>
//                    <dd className="col-sm-10">{lease.price}</dd>

//                    <dt className="col-sm-2">Status</dt>
//                    <dd className="col-sm-10">{lease.status}</dd>
//                </dl>
//            </div>
//    );

//    async function getLease(leaseId: number) {
//        const response = await fetch('/lease/' + leaseId);
//        //console.log(`\n************\n ${response.body} \n ***************8`)
//        if (response.ok) {
//            const data = await response.json();
//            console.log(data)
//            setLease(data);
//        }
//    }
//}

//export const LeaseDetails = () => {
//    const { leaseId } = useParams()
//    const { username, isadmin } = useAuth();
//    //const detailsBlock = 
//    const header = Header();
//    const footer = Footer();
//    return (
//    <div>
//        {header}
//            <div>
//                <h1>Details</h1>
//                {LeaseDetailsBlock(leaseId!)}
//                <div>
//                </div>
//        </div>
//        {footer}
//    </div>
//    )
//}

//export const LeaseDelete = () => {
//    const { leaseId } = useParams()
//    const [feedback, setFeedback] = useState<JSX.Element>(<div></div>)
//    //const detailsBlock = 
//    const header = Header();
//    const footer = Footer();
//    const { isadmin } = useAuth();
//    return (isadmin?
//        <div>
//            {header}
//            <div>
//                <h1>Delete</h1>
//                {feedback}
//                {LeaseDetailsBlock(leaseId!)}
//                <div>
//                    {/*Conditional rendering based on user role and authentication */}
//                    {/*{user?.isAuthenticated && !user?.roles.includes('Admin') && lease?.id && (*/}
//                    <button onClick={() => DeleteConfirmed(parseInt(leaseId!))} className="btn btn-danger">
//                        Delete
//                    </button>
//                    {/*)}*/}
//                    {' | '}
//                    <Link to={'/catalogue'}>Back to list</Link>
//                </div>
//            </div>
//            {footer}
//        </div>
//        : <Navigate to="/catalogue" />
//    );

//    async function DeleteConfirmed(leaseId: number) {
//        console.log(`deleting lease of id {leaseID}`);
//        //make api call to lease/id as http delete
//        const requestOptions = {
//            method: 'DELETE',
//            headers: { 'Content-Type': 'application/json' },
//        };
//        const response = await fetch('/lease/' + leaseId, requestOptions);
//        if (response.status == 403) {
//            setFeedback(<div style={{ color: "red" }} >Cannot delete leased lease</div>);
//        }
//        else if (response.status == 202) {
//            setFeedback(<div style={{ color: "green" }} >Lease hidden</div>);
//        }
//        else if (response.ok) {
//            setFeedback(<div style={{ color: "green" }} >Lease deleted</div>);
//        }
//        else {
//            setFeedback(<div style={{ color: "red" }} >Lease does not exist or different error occured</div>);
//        }
//    }
//}





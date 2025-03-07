import { useEffect, useState } from 'react';
import { Header, Footer } from '../shared/_Layout';
import { useAuth } from '../shared/AuthProvider';
import { Link, Navigate } from 'react-router-dom';
import { Book } from "../books/Catalogue"
import { User } from "../users/UsersList"

// Define the Lease interface based on the model
export interface Lease {
    id: number;
    leaseStart?: string;
    leaseEnd?: string;
    book: Book;
    user: User;
    type: string;
    active: boolean;
    rowVersion: string;
}

// Define the Props interface for the component
//interface LeaseTableProps {
//    leases: Lease[];
//    isAdmin: boolean; // Assuming you pass whether the user is an admin or not
//}

const LeaseTable = ( showInactive = false ) => {
    const [leases, setLeases] = useState<Lease[]>();
    const [feedback, setFeedback] = useState<JSX.Element>(<div></div>)


    useEffect(() => {
        populateLeaseData();
    }, []);

    async function populateLeaseData() {
        let response
        if (showInactive) {
            response = await fetch('/leases/inactive');
        }
        else {
            response = await fetch('leases');
        }
        console.log(response)
        if (response.ok) {
            const data = await response.json();
            console.log(data)
            setLeases(data);
        }
    }

    return (leases === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started</em></p>
        :
        <>
            {feedback}
            <table className="table">
                <thead>
                    <tr>
                        <th>Book</th>
                        <th>User</th>
                        <th>Start date</th>
                        <th>End date</th>
                        <th>Type</th>

                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {leases.map((lease) => (
                        <tr key={lease.id}>
                            <td>{lease.book.title}</td>
                            <td>{lease.user.userName}</td>
                            <td>{lease.leaseStart}</td>
                            <td>{lease.leaseEnd}</td>
                            <td>{lease.type}</td>
                            <td>
                                {showInactive ?
                                    <>
                                        <button onClick={() => DeleteConfirmed(lease.id)} className="btn btn-danger">
                                            Delete
                                        </button>
                                    </>
                                    :
                                    <>
                                        <Link to={`/editLease/${lease.id}`}>Edit</Link>
                                    </>
                                }
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </>


    );
    async function DeleteConfirmed(leaseId: number) {
        console.log(`deleting lease of id ${leaseId}`);
        //make api call to lease/id as http delete
        const requestOptions = {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
        };
        const response = await fetch('/leases/delete/' + leaseId, requestOptions);
        if (response.status == 404) {
            setFeedback(<div style={{ color: "red" }} >Lease {leaseId} does not exist </div>);
        }
        else if (response.status == 400) {
            setFeedback(<div style={{ color: "green" }} >NO LEASE TABLE</div>);
        }
        else if (response.ok) {
            //await response.json()
            setFeedback(<div style={{ color: "green" }} >Lease deleted</div>);
            populateLeaseData() 
        }
    }
};


export const LeaseList = () => {
    const { isadmin } = useAuth();
    const header = Header();
    const footer = Footer();
    return (
        isadmin ?
            <div>
                {header}
                <Link to='/book-leases/inactive'>
                    See inactive
                </Link>

                <div>
                    <h1>Leases List</h1>
                    {LeaseTable(false)}
                </div>
                {footer}
            </div>
            : <Navigate to="/" />
    );
}

export const LeaseListInactive = () => {
    const { isadmin } = useAuth();
    const header = Header();
    const footer = Footer();
    return (
        isadmin ?
            <div>
                {header}
                <Link to="/book-leases">
                    See active
                </Link>

                <div>
                    <h1>Leases List</h1>
                    {LeaseTable(true)}
                </div>
                {footer}
            </div>
            : <Navigate to="/" />
    );
}

//export default LeaseTable;
//{ isAdmin && <td>{lease.hidden ? 'Yes' : 'No'}</td> }
//<td>
//    <a href={`/details/${lease.id}`}>Details</a> |
//    {isAdmin && (
//        <>
            //<a href={`/edit/${lease.id}`}>Edit</a> |
//            <a href={`/delete/${lease.id}`}>Delete</a>
//        </>
//    )}
//</td>
//{ isAdmin && <th>Hidden</th> }
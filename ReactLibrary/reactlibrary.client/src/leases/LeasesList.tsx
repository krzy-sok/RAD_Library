import { useEffect, useState } from 'react';
import { Header, Footer } from '../shared/_Layout';
import { useAuth } from '../shared/AuthProvider';
import { Link, Navigate } from 'react-router-dom';
import { Book } from "../books/Catalogue"
import { User } from "../users/UsersList"

// Define the Lease interface based on the model
export interface Lease {
    Id: number;
    leaseStart?: string;
    leaseEnd?: string;
    book: Book;
    user: User;
    type: string;
    active: boolean;
}

// Define the Props interface for the component
//interface LeaseTableProps {
//    leases: Lease[];
//    isAdmin: boolean; // Assuming you pass whether the user is an admin or not
//}

const LeaseTable = () => {
    const [leases, setLeases] = useState<Lease[]>();
    const { isadmin } = useAuth();
    const [ showInactive, setShowInactive ] = useState<boolean>(false);

    useEffect(() => {
        populateLeaseData();
    }, []);

    const contents = leases === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started</em></p>
        :
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
                    <tr key={lease.Id}>
                        <td>{lease.book.title}</td>
                        <td>{lease.user.userName}</td>
                        <td>{lease.leaseStart}</td>
                        <td>{lease.leaseEnd}</td>
                        <td>{lease.type}</td>
                        <td>
                        <>
                            <Link to={`/editLease/${lease.Id}`}>Edit</Link> |
                            <Link to={`/deleteLease/${lease.Id}`}>Delete</Link>
                        </>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>;


    const header = Header();
    const footer = Footer();

    const changeInactive = () => {
        setShowInactive(!showInactive)
    }

    return ( isadmin?
        <div>
            {header}
            <button className="btn btn-primary" onClick={changeInactive}>
                See {showInactive ? <>active</>  : <>inactive</> }
            </button>
            <div>
                <h1>Leases List</h1>
                {contents}
            </div>
            {footer}
        </div>
        : <Navigate to="/" />
    );


    async function populateLeaseData() {
        let response
        if (showInactive) {
            response = await fetch('leases');
        }
        else {
            response = await fetch('leases/inactive');
        }
        console.log(response)
        if (response.ok) {
            const data = await response.json();
            console.log(data)
            setLeases(data);
        }
    }
};


export const LeaseList = () => {
    return LeaseTable()
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
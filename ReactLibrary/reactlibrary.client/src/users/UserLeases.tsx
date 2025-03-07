import { useEffect, useState } from 'react';
import { Link, Navigate } from 'react-router-dom';
import { Lease } from "../leases/LeasesList"
import { Header, Footer } from '../shared/_Layout';
import { useAuth } from '../shared/AuthProvider';


const LeaseTable = () => {
    const [leases, setLeases] = useState<Lease[]>();
    const { username } = useAuth();

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
                    <tr key={lease.id}>
                        <td>{lease.book.title}</td>
                        <td>{lease.user.userName}</td>
                        <td>{lease.leaseStart}</td>
                        <td>{lease.leaseEnd}</td>
                        <td>{lease.type}</td>
                        <td>
                            <Link to={`/leaseDetails/${lease.id}`}>Details</Link>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>;


    const header = Header();
    const footer = Footer();


    return (username?
        <div>
            {header}
            <div>
                <h1>Leases List</h1>
                {contents}
            </div>
            {footer}
        </div>
        : <Navigate to="/"/>
    );


    async function populateLeaseData() {
        const response = await fetch('user/leases');
        if (response.ok) {
            const data = await response.json();
            console.log(data)
            setLeases(data);
        }
    }
};


export const UserLeases = () => {
    return LeaseTable()
}
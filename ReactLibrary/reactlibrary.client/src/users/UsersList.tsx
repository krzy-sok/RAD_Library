import { useEffect, useState } from 'react';
import { Header, Footer } from '../shared/_Layout';
import { useAuth } from '../shared/AuthProvider';
import { Navigate } from 'react-router-dom';

// Define the User interface based on the model
export interface User {
    Id: number,
    userName: string,
    firstName?: string,
    lastName?: string,
    email: string,
    phoneNumber: string,
}

// Define the Props interface for the component
//interface UserTableProps {
//    users: User[];
//    isAdmin: boolean; // Assuming you pass whether the user is an admin or not
//}

const UserTable = () => {
    const [users, setUsers] = useState<User[]>();
    const { isadmin } = useAuth();

    useEffect(() => {
        populateUserData();
    }, []);

    const contents = users === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started</em></p>
        :
        <table className="table">
            <thead>
                <tr>
                    <th>User Name</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Email</th>
                    <th>Phone Number</th>
                </tr>
            </thead>
            <tbody>
                {users.map((user) => (
                    <tr key={user.Id}>
                        <td>{user.userName}</td>
                        <td>{user.firstName}</td>
                        <td>{user.lastName}</td>
                        <td>{user.email}</td>
                        <td>{user.phoneNumber}</td>
                        {/*<td>*/}
                        {/*    <Link to={`/detailsUser/${user.Id}`}>Details</Link>*/}
                        {/*</td>*/}
                    </tr>
                ))}
            </tbody>
        </table>;


    const header = Header();
    const footer = Footer();

    return (isadmin? 
        <div>
            {header}
            <div>
                <h1>Users List</h1>
                {contents}
            </div>
            {footer}
        </div>
        : <Navigate to="/"/>
    );

    async function populateUserData() {
        const response = await fetch('/users');
        console.log(response)
        if (response.ok) {
            const data = await response.json();
            console.log(data)
            setUsers(data);
        }
    }
};


export const UsersList = () => {
    return UserTable()
}

//export default UserTable;
//{ isAdmin && <td>{user.hidden ? 'Yes' : 'No'}</td> }
//<td>
//    <a href={`/details/${user.id}`}>Details</a> |
//    {isAdmin && (
//        <>
            //<a href={`/edit/${user.id}`}>Edit</a> |
//            <a href={`/delete/${user.id}`}>Delete</a>
//        </>
//    )}
//</td>
//{ isAdmin && <th>Hidden</th> }
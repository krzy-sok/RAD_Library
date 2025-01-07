import React, { useEffect, useState } from 'react';
import { Header, Footer } from '../shared/_Layout'
import { useParams } from 'react-router-dom';
import { User } from "./UsersList"
import { useAuth } from '../shared/AuthProvider';
import { Link, Navigate} from 'react-router-dom';

export const UserDetailsBlock = (userId: string) => {
    const [user, setUser] = useState<User>()

    useEffect(() => {
        getUser(parseInt(userId!))
    }, [userId]);

    console.log(user)
    //const details = user === undefined ?
    //    <p>no user</p>
    //    :
    return (
        user === undefined ? <p>no user</p> :
            <div>
                <h4>User</h4>
                <hr />
                <dl className="row">
                    <dt className="col-sm-2">Title</dt>
                    <dd className="col-sm-10">{user.userName}</dd>

                    <dt className="col-sm-2">Author</dt>
                    <dd className="col-sm-10">{user.first}</dd>

                    <dt className="col-sm-2">Publisher</dt>
                    <dd className="col-sm-10">{user.publisher}</dd>

                    <dt className="col-sm-2">Publication Date</dt>
                    <dd className="col-sm-10">{user.publicationDate}</dd>

                    <dt className="col-sm-2">Price</dt>
                    <dd className="col-sm-10">{user.price}</dd>

                    <dt className="col-sm-2">Status</dt>
                    <dd className="col-sm-10">{user.status}</dd>
                </dl>
            </div>
    );

    async function getUser(userId: number) {
        const response = await fetch('/user/' + userId);
        //console.log(`\n************\n ${response.body} \n ***************8`)
        if (response.ok) {
            const data = await response.json();
            console.log(data)
            setUser(data);
        }
    }
}

export const UserDetails = () => {
    const { userId } = useParams()
    const { username, isadmin } = useAuth();
    //const detailsBlock = 
    const header = Header();
    const footer = Footer();
    return (
    <div>
        {header}
            <div>
                <h1>Details</h1>
                {UserDetailsBlock(userId!)}
                <div>
                    {!isadmin && username ?
                        <button onClick={() => onReserve(userId!)} className="btn btn-primary">
                            Reserve
                        </button>
                        : null
                    }
                    {isadmin ? 
                        <Link to={`/editUser/${userId}`}>Edit</Link>
                        : null
                    }
                    {' | '}
                    <Link to={'/catalogue'}>Back to list</Link>
                </div>
        </div>
        {footer}
    </div>
    )

    function onReserve(userId: string) {
        console.log('reserving');
        return
    }
}

export const UserDelete = () => {
    const { userId } = useParams()
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
                {UserDetailsBlock(userId!)}
                <div>
                    {/*Conditional rendering based on user role and authentication */}
                    {/*{user?.isAuthenticated && !user?.roles.includes('Admin') && user?.id && (*/}
                    <button onClick={() => DeleteConfirmed(parseInt(userId!))} className="btn btn-danger">
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

    async function DeleteConfirmed(userId: number) {
        console.log(`deleting user of id {userID}`);
        //make api call to user/id as http delete
        const requestOptions = {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
        };
        const response = await fetch('/user/' + userId, requestOptions);
        if (response.status == 403) {
            setFeedback(<div style={{ color: "red" }} >Cannot delete leased user</div>);
        }
        else if (response.status == 202) {
            setFeedback(<div style={{ color: "green" }} >User hidden</div>);
        }
        else if (response.ok) {
            setFeedback(<div style={{ color: "green" }} >User deleted</div>);
        }
        else {
            setFeedback(<div style={{ color: "red" }} >User does not exist or different error occured</div>);
        }
    }
}





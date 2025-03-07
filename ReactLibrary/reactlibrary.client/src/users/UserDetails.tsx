import React, { useEffect, useState } from 'react';
import { Header, Footer } from '../shared/_Layout'
import { useParams } from 'react-router-dom';
import { User } from "./UsersList"
import { useAuth } from '../shared/AuthProvider';
import { Link, Navigate} from 'react-router-dom';

export const UserDetailsBlock = () => {
    const [user, setUser] = useState<User>()

    useEffect(() => {
        getUser()
    }, []);

    console.log(user)
    //const details = user === undefined ?
    //    <p>no user</p>
    //    :
    return (
        user? 
            <div>
                <h4>User</h4>
                <hr />
                <dl className="row">
                    <dt className="col-sm-2">Title</dt>
                    <dd className="col-sm-10">{user.userName}</dd>

                    <dt className="col-sm-2">Author</dt>
                    <dd className="col-sm-10">{user.firstName}</dd>

                    <dt className="col-sm-2">Publisher</dt>
                    <dd className="col-sm-10">{user.lastName}</dd>

                    <dt className="col-sm-2">Publication Date</dt>
                    <dd className="col-sm-10">{user.email}</dd>

                    <dt className="col-sm-2">Price</dt>
                    <dd className="col-sm-10">{user.phoneNumber}</dd>
                </dl>
            </div >
           : <div>Hold on while we fetch data</div>  
    );

    async function getUser() {
        const response = await fetch('/user/details');
        //console.log(`\n************\n ${response.body} \n ***************8`)
        if (response.ok) {
            const data = await response.json();
            console.log(data)
            setUser(data);
        }
        
    }
}

export const UserDetails = () => {
    const { username, isadmin } = useAuth();
    //const detailsBlock = 
    const header = Header();
    const footer = Footer();
    return (
    <div>
        {header}
            <div>
                <h1>Details</h1>
                {UserDetailsBlock()}
                <div>
                    {!isadmin && username ?
                        <>
                            <Link to="/user-leases" > View leases</Link> |
                            <Link to="/user/delete" > Delete account</Link>
                        </>
                        : null
                    }
                </div>
        </div>
        {footer}
    </div>
    )

}





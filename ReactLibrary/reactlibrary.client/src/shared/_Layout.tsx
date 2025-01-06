import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

//interface LayoutProps {
//    children: React.ReactNode;
//    title?: string;
//    result?: string;
//    error?: string;
//    isAdmin: boolean;
//}
//{ children, title = "Library", result, error, isAdmin }

interface UserInfo {
    userName: string;
    email: string;
}

export const Header =  () => {
    const title = "ReactLibarary"

    //const loginPartial = LoginPartial();
    return (
        <header>
            <nav className="navbar navbar-expand-sm navbar-light bg-white border-bottom box-shadow mb-3">
                <div className="container-fluid">
                    <Link className="navbar-brand" to="/">{title}</Link>
                    <button
                        className="navbar-toggler"
                        type="button"
                        data-bs-toggle="collapse"
                        data-bs-target=".navbar-collapse"
                        aria-controls="navbarSupportedContent"
                        aria-expanded="false"
                        aria-label="Toggle navigation"
                    >
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                        <ul className="navbar-nav flex-grow-1">
                            <li className="nav-item">
                                <Link className="nav-link text-dark" to="/">Home</Link>
                            </li>
                            <li className="nav-item">
                                <Link className="nav-link text-dark" to="/privacy">Privacy</Link>
                            </li>
                            <li className="nav-item">
                                <Link className="nav-link text-dark" to="/catalogue">Catalogue</Link>
                            </li>
                            {/*{isAdmin && (*/}
                            {/*    <>*/}
                            {/*        <li className="nav-item">*/}
                            {/*            <Link className="nav-link text-dark" to="/book-leases">Book Leases</Link>*/}
                            {/*        </li>*/}
                            {/*        <li className="nav-item">*/}
                            {/*            <Link className="nav-link text-dark" to="/library-users">Library Users</Link>*/}
                            {/*        </li>*/}
                            {/*    </>*/}
                            {/*)}*/}
                        </ul>
                         {/*Replace with your Login component logic */}
                        <LoginPartial />

                    </div>
                </div>
            </nav>
        </header>
   )
}

export const Footer= () => {
    return (
        <footer className="border-top footer text-muted">
            <div className="container">
                &copy; 2024 - Library - <Link to="/privacy">Privacy</Link>
            </div>
        </footer>
    );
}

export const LoginPartial = () => {
    const [userInfo, setUserInfo] = useState<UserInfo>();

    useEffect(() => {
        checkIfLoggedIn();
    }, []);

    return (
        userInfo === undefined
            ? <ul className="nav navbar-nav navbar-right">
                <li>
                    <Link className="nav-link text-dark" to="/registration">Register</Link>
                </li>
                <li>
                    <Link className="nav-link text-dark" to="/login">Log in</Link>
                </li>
            </ul>
            : <ul className="nav navbar-nav navbar-right">
                <li>
                    <h4>Hello {userInfo.userName} </h4>
                </li>
                <li>
                    <button className="btn btn-link navbar-btn navbar-link" onClick={Logout}>Log out</button>
                </li>
            </ul> 
    );

    async function checkIfLoggedIn() {
        const response = await fetch("/user/info")
        if (response.ok) {
            const data = await response.json();
            setUserInfo(data);
        }
    }

    async function Logout() {

    }
}

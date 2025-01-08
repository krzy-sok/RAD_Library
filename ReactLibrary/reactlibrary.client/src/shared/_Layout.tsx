import { useState } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from './AuthProvider'


export const Header =  () => {
    const title = "ReactLibarary"
    const { isadmin } = useAuth();

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
                                <Link className="nav-link text-dark" to="/catalogue">Catalogue</Link>
                            </li>
                            {isadmin ? 
                                <>
                                    <li className="nav-item">
                                        <Link className="nav-link text-dark" to="/book-leases">Book Leases</Link>
                                    </li>
                                    <li className="nav-item">
                                        <Link className="nav-link text-dark" to="/library-users">Library Users</Link>
                                    </li>
                                </>
                                : null
                            }
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
    const { username, handleLogout } = useAuth();

    return (
        username === undefined || username == null
            ? <ul className="nav navbar-nav navbar-right">
                <li>
                    <Link className="nav-link text-dark" to="/registration">Register</Link>
                </li>
                <li>
                    <Link className="nav-link text-dark" to="/login">Log in</Link>
                </li>
            </ul>
            : <ul className="nav navbar-nav navbar-right">
                <Link to="/user">
                    Hello {username}       
                </Link>
                <li>
                    <button className="btn btn-link navbar-btn navbar-link" onClick={handleLogout}>Log out</button>
                </li>
            </ul> 
    );
}

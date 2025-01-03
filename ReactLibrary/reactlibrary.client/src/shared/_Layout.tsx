import React from 'react';
import { Link } from 'react-router-dom';

//interface LayoutProps {
//    children: React.ReactNode;
//    title?: string;
//    result?: string;
//    error?: string;
//    isAdmin: boolean;
//}
//{ children, title = "Library", result, error, isAdmin }

export const Header =  () => {
    const title = "ReactLibarary"
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
                         {/*<LoginPartial /> */}
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

//export const Feedback = () => {
//    return (
//        <div className="container">
//            {result && 
//                <div className="form-group" style={{ color: 'green' }}>
//                    {result}
//                </div>
//            )}
//            {error && 
//                <div className="form-group" style={{ color: 'red' }}>
//                    {error}
//                </div>
//            )}
//            <main role="main" className="pb-3">
//                {children}
//            </main>
//        </div>
//    )
//}

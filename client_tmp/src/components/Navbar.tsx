import React, {useContext} from 'react';
import {NavLink} from "react-router-dom";
import {AuthContext} from "../authorization/Authorization";

export const Navbar : React.FunctionComponent = () => {
    const {logout} = useContext(AuthContext);
    const logoutHandler = async (event: React.MouseEvent<HTMLAnchorElement>) => {
        await logout();
    };


    return (
        <nav className="navbar navbar-light bg-light pt-0">
            <div className="container-fluid bg-primary bg-gradient">
                <NavLink className="navbar-brand" to="/">
                    <img src="logo.png" alt="logo" width={100} height={70}/>
                </NavLink>
                <ul className="nav nav-pills nav-fill">
                    <li className="nav-item">
                        <NavLink className="nav-link active" aria-current="page" to="/">Current User</NavLink>
                    </li>
                    <li className="nav-item">
                        <NavLink className="nav-link" onClick={logoutHandler} to="/">Log out</NavLink>
                    </li>
                </ul>
            </div>
        </nav>
    );
}
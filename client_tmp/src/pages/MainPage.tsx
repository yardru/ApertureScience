import React, {useContext} from "react";
import {Navbar} from "../components/Navbar";
import {Search} from "../components/Search";
import {EmployeeList} from "../components/EmployeeList";
import {AuthContext} from "../authorization/Authorization";


export const MainPage: React.FunctionComponent = () => {
    return (
        <div className="App">
            < Navbar />
            {useContext(AuthContext).state?.accessToken}
            < Search />
            < EmployeeList employees={[]} />
        </div>
    );
}

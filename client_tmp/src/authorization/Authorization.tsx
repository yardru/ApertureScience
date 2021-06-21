import React, {createContext} from 'react'
import {useAuth} from "./useAuth";
import {AuthRouter} from "./AuthRouter";

export const AuthContext = createContext({} as ReturnType<typeof useAuth>);
export const Authorization: React.FC = ({children}) => {
    const {state, login, logout} = useAuth();
    return (
        <AuthContext.Provider value={{state, login, logout}}>
            <AuthRouter isAuth={!!state}>
                {children}
            </AuthRouter>
        </AuthContext.Provider>
    )
}

import React from 'react'
import {Switch, Route, Redirect} from 'react-router-dom'
import {AuthPage} from '../pages/AuthPage'

interface AuthRouterProps {
    isAuth: boolean;
}

export const AuthRouter: React.FC<AuthRouterProps> = ({isAuth, children}) => {
    if (isAuth) {
        return (
            <div>
                {children}
            </div>
        )
    }

    return (
        <Switch>
            <Route path="/" exact>
                <AuthPage />
            </Route>
            <Redirect to="/" />
        </Switch>
    )
}


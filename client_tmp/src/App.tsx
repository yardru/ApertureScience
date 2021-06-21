import React from 'react';
import { BrowserRouter, Switch, Route} from 'react-router-dom'
import {MainPage} from "./pages/MainPage";
import {Authorization} from "./authorization/Authorization";

const App: React.FunctionComponent = () => {
    return (
        <BrowserRouter>
            <Authorization>
                <div className="container">
                    <Switch>
                        <Route component={MainPage} path="/" exact />
                    </Switch>
                </div>
            </Authorization>
        </BrowserRouter>
  );
}

export default App;

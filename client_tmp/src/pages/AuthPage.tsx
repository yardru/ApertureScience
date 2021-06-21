import React, {useContext, useState} from "react";
import {AuthContext} from "../authorization/Authorization";


export const AuthPage: React.FunctionComponent = () => {
    const [email, setEmail] = useState<string>('')
    const [password, setPassword] = useState<string>('')
    const {login} = useContext(AuthContext);

    const emailHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
        setEmail(event.target.value);
    };

    const passwordHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
        setPassword(event.target.value);
    };

    const submitHandler = async (event: React.MouseEvent<HTMLButtonElement>) => {
        if (await login(email, password)) {
            setEmail('');
            setPassword('');
            return;
        }
        alert("submitHandler: Invalid email or password")
    };

    return (
        <div className="card">
            <div className="card-header">
                Authorization
            </div>
            <div className="card-body">
                <div className="input-group">
                    <div className='container'>
                        <input
                            value={email}
                            onChange={emailHandler}
                            placeholder='Email'
                            id='Email'
                            name='email'
                            type='text'
                            className='row'
                        />
                        <input
                            value={password}
                            onChange={passwordHandler}
                            placeholder='Password'
                            id='password'
                            name='password'
                            type='password'
                            className='row'
                        />
                        <button
                            id='submit'
                            name='submit'
                            className='btn btn-primary'
                            type='submit'
                            value='Login'
                            onClick={submitHandler}
                        >Log in</button>
                    </div>
                </div>
            </div>
        </div>
    );
}

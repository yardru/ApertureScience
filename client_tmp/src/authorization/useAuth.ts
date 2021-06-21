import {useCallback, useEffect, useState} from "react";
import {AuthState} from "../types/AuthState";
import AuthService from "../services/AuthService";

const storageName = 'userData'

export const useAuth = () => {
    const [state, setState] = useState<AuthState | null>(null);

    const login = useCallback(  async (email: string, password: string) => {
        const response: AuthState | null = await AuthService.login(email, password);
        if (!response)
            return false;

        localStorage.setItem(storageName, JSON.stringify(response));
        setState(response);
        return true;
    }, []);

    const logout = useCallback(  async () => {
        localStorage.removeItem(storageName);
        setState(null);
    }, []);

    useEffect(() => {
        const lastState = localStorage.getItem(storageName);
        if (lastState)
            setState(JSON.parse(lastState));
    }, []);

    return {state, login, logout};
}
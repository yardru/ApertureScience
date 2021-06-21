import axios from "axios";
import {apiUrl, authRoute} from "./Routs";
import {AuthState} from "../types/AuthState";
import qs from "qs";

const Api = axios.create({
    baseURL: apiUrl + authRoute,
    headers: {'Content-Type': 'application/x-www-form-urlencoded'},
});

export default class AuthService {
    static async login(email: string, password: string): Promise<AuthState | null> {
        try {
            const response = await Api.post("/", qs.stringify({
                email: 'xxx@yyy.com',
                password: 'drowssap'
            }));
            return response.data;
        } catch (error) {
            console.log(error)
        }
        return null;
    }
}

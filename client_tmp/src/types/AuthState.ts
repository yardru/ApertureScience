import {EmployeeRole} from "./EmployeeRole";

export type AuthState = {
    userId: number;
    userRole: EmployeeRole;
    accessToken: string;
}

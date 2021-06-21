import {EmployeeRole} from "./EmployeeRole";

export type Employee = {
    id: number;
    email: string;
    role: EmployeeRole;
    firstName: string;
    lastName: string;
    phone: string;
}

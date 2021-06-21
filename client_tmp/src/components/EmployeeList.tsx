import React from 'react';
import {Employee} from "../types/Employee";

interface EmployeeListProps {
    employees: Employee[];
};

export const EmployeeList = ({employees} : EmployeeListProps) => {
    const [count, setCount] = React.useState<number>(5);
    const [page, setPage] = React.useState<number>(1);

    const countSelectorHandler = (event: React.ChangeEvent<HTMLSelectElement>) => {
        console.log(event.target.value);
        setCount(Number(event.target.value));
        setPage(1);
    };

    return (
        <div>
            <div className="list-group ">
                {
                    employees.slice((page - 1) * count, page * count).map(employee => {
                       return (
                           <button type="button" className="bg-dark list-group-item list-group-item-action" key={employee.id}>
                               {employee.firstName} {employee.lastName}
                           </button>
                       );
                    })
                }
            </div>
            <select onChange={countSelectorHandler} className="form-select" aria-label="Default select example">
                <option selected>5</option>
                <option>10</option>
                <option>25</option>
                <option>50</option>
            </select>
        </div>
    );
}

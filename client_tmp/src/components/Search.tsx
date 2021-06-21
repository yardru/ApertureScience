import React from 'react';

/*
interface SearchProps {
    Filter(employee: Employee): boolean;
}
*/
export const Search : React.FunctionComponent = () => {
    /*let radios: {id: number, value: string}[] = [
        { "id": 0, "value": "FirstName" },
        { "id": 1, "value": "LastName" },
        { "id": 2, "value": "Email" }
    ];*/
    const searchHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
        console.log(event.target.value);
    };

    return (
        <div className="container-fluid">
            <input onChange={searchHandler} className="form-control" type="text" placeholder="Search..."/>
            <div className="form-check form-check-inline">
                <input className="form-check-input" type="radio" name="inlineRadioOptions" id="inlineRadio1"
                       value="option1 " />
                    <label className="form-check-label" htmlFor="inlineRadio1">First Name</label>
            </div>
            <div className="form-check form-check-inline">
                <input className="form-check-input" type="radio" name="inlineRadioOptions" id="inlineRadio2"
                       value="option2"/>
                    <label className="form-check-label" htmlFor="inlineRadio2">Last Name</label>
            </div>
            <div className="form-check form-check-inline">
                <input className="form-check-input" type="radio" name="inlineRadioOptions" id="inlineRadio3"
                       value="option3"/>
                    <label className="form-check-label" htmlFor="inlineRadio3">email</label>
            </div>
        </div>
    );
}

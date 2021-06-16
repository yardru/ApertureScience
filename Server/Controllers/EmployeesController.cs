using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ApertureScience
{
    [Route("employees")]
    [ApiController]
    public class EmployeesController : AuthorizedBaseController
    {
        private readonly EmployeesManager employees;

        public EmployeesController(EmployeesManager context, IAuthorizationService authorizationService)
        {
            employees = context;
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Object>> PostEmployee(Employee employee)
        {
            if (UserRole <= employee.Role)
                return Forbid();

            if ((employees.GetAll().Any(other => other.Email == employee.Email)))
                return BadRequest("Email should be unique");

            await employees.AddAsync(employee);
            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee.Reduce());
        }

        [HttpGet]
        public ActionResult<IEnumerable<Object>> GetEmployee()
        {
            return Ok(employees.GetAll().Select(employee => employee.Reduce()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetEmployee(int id)
        {
            var employee = await employees.GetAsync(id);
            if (employee == null)
                return NotFound();

            return employee.Reduce();
        }


        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutEmployee(int id, Employee employee)
        {
            var oldRole = employees.GetRole(id);
            if (oldRole == Employee.Roles.UNDEFINED)
                return NotFound();

            if ((UserId == employee.Id && UserRole != employee.Role) || UserRole <= oldRole || UserRole <= employee.Role)
                return Forbid();

            if ((employees.GetAll().Any(other => (other.Email == employee.Email) && (other.Id != employee.Id))))
                return BadRequest("Email should be unique");

            await employees.UpdateAsync(employee);
            return NoContent();
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var employee = await employees.GetAsync(id);
            if (employee == null)
                return NotFound();

            if (UserRole <= employee.Role)
                return Forbid();

            await employees.RemoveAsync(employee);

            return NoContent();
        }
    }
}

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
        private readonly ServerContext dataBase;
        IAuthorizationService _authorizationService;

        public EmployeesController(ServerContext context, IAuthorizationService authorizationService)
        {
            dataBase = context;
            _authorizationService = authorizationService;
        }

        // GET: api/Employees
        //[JsonReduceResultFilter]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Object>>> GetEmployee()
        {
            return Ok((await dataBase.GetEmployeesAsync()).Select(employee => employee.Reduce()));
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Object>> PostEmployee(Employee employee)
        {
            if (UserRole <= employee.Role)
                return Forbid();

            if (employee.PhotoNamesList.Count > 0)
                return BadRequest(new { PhotoNamesList = "To change photos use .../{id}/photos" });

            await dataBase.AddEmployeeAsync(employee);
            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee.Reduce());
        }

        // GET: api/Employees/5
        //[JsonReduceResultFilter]
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetEmployee(int id)
        {
            var employee = await dataBase.GetEmployeeAsync(id);
            if (employee == null)
                return NotFound();

            return employee.Reduce();
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutEmployee(int id, Employee employee)
        {
            /**/
            var oldEmployee = await dataBase.GetEmployeeAsyncAsNoTracking(employee.Id);
            
            if (oldEmployee == null)
                return NotFound();

            if ((UserId == employee.Id && UserRole != employee.Role) || UserRole <= oldEmployee.Role || UserRole <= employee.Role)
                return Forbid();

            if (employee.PhotoNamesList.Count != oldEmployee.PhotoNamesList.Count || employee.PhotoNamesList.Any(pn => !oldEmployee.PhotoNamesList.Contains(pn)))
                return BadRequest("To change photos use .../{id}/photos");
            /**/
            await dataBase.UpdateEmployeeAsync(employee);
            return NoContent();
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var employee = await dataBase.GetEmployeeAsyncAsNoTracking(id);
            if (employee == null)
                return NotFound();

            if (UserRole <= employee.Role)
                return Forbid();

            await dataBase.RemoveEmployeeAsync(employee);

            return NoContent();
        }
    }
}

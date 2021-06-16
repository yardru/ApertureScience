using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience
{
    [Route("employees/{employeeId}/photos")]
    [ApiController]
    public class PhotoController : AuthorizedBaseController
    {
        public PhotoController(ServerContext context, IWebHostEnvironment appEnvironment)
        {
            dataBase = context;
            _appEnvironment = appEnvironment;
        }
        
        [HttpGet("{fileName}")]
        public ActionResult Get(int employeeId, string fileName)
        {
            var proc = PhotoProcessor.GetProcessor(fileName);

            if (!proc.IsExists())
                return NotFound();

            return File(proc.Get(), proc.ContentType);
        }

        [HttpPost]
        public async Task<ActionResult> Post(int employeeId, [FromForm] IFormFileCollection uploads)
        {
            uploads = HttpContext.Request.Form.Files; /// WARNING!!! uploads from argument's don't have anything
            var employee = await dataBase.GetEmployeeAsyncAsNoTracking(employeeId);

            if (uploads == null || uploads.Count == 0)
                return BadRequest();

            if (employee == null)
                return NotFound();

            if (UserRole <= employee.Role && UserId != employee.Id)
                return Forbid();

            if (employee.PhotoNamesList.Count >= MAX_IMAGE_NUMBER)
                return Conflict();

            /**/
            foreach (var upload in uploads)
            {
                var proc = PhotoProcessor.GetProcessor(upload.FileName);
                if (!proc)
                    return BadRequest();

                await proc.Save(upload);
                employee.PhotoNamesList.Append(proc.FileName);
            }
            /**/

            await dataBase.UpdateEmployeeAsync(employee);

            return Accepted();
        }

        [HttpDelete("{fileName}")]
        public async Task<ActionResult> Delete(int employeeId, string fileName)
        {
            fileName = fileName.ToLower();
            var employee = await dataBase.GetEmployeeAsyncAsNoTracking(employeeId);
            if (employee == null)
                return NotFound();

            if (UserRole <= employee.Role && UserId != employee.Id)
                return Forbid();

            if (employee.PhotoNamesList.Remove(fileName))
                await dataBase.UpdateEmployeeAsync(employee);

            PhotoProcessor.GetProcessor(fileName).Delete();

            return NoContent();
        }

        private readonly ServerContext dataBase;
        private readonly IWebHostEnvironment _appEnvironment;

        static private readonly int MAX_IMAGE_NUMBER = 3;
    }
}

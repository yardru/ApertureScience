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
        public PhotoController(EmployeesManager context, IWebHostEnvironment appEnvironment)
        {
            employees = context;
       }

        [HttpGet]
        public ActionResult<IEnumerable<Photo>> Get(int employeeId)
        {
            return Ok(PhotosManager.Photos(employeeId).Select(photo => photo.FileName));
        }

        [HttpGet("{fileName}")]
        public ActionResult Get(int employeeId, string fileName)
        {
            var photo = PhotosManager.Photo(employeeId, fileName);

            if (!PhotosManager.IsExists(photo))
                return NotFound();

            return File(PhotosManager.Load(photo), photo.ContentType);
        }

        [HttpPost]
        public async Task<ActionResult> Post(int employeeId, [FromForm] IFormFileCollection _uploads)
        {

            var uploads = HttpContext.Request.Form.Files; /// WARNING!!! uploads from argument's don't have anything
            var role = employees.GetRole(employeeId);
            if (role == Employee.Roles.UNDEFINED)
                return NotFound();

            if (uploads == null || uploads.Count == 0)
                return BadRequest();

            if (UserRole <= role && UserId != employeeId)
                return Forbid();

            foreach (var upload in uploads)
            {
                var photo = PhotosManager.Photo(employeeId, upload.FileName);
                if (!photo)
                    return BadRequest();

                await PhotosManager.Save(photo, upload);
            }

            return Accepted();
        }

        [HttpDelete("{fileName}")]
        public async Task<ActionResult> Delete(int employeeId, string fileName)
        {
            var employee = await employees.GetAsync(employeeId);
            if (employee == null)
                return NotFound();

            if (UserRole <= employee.Role && UserId != employee.Id)
                return Forbid();

            PhotosManager.Delete(PhotosManager.Photo(employeeId, fileName));

            return NoContent();
        }

        private readonly EmployeesManager employees;
    }
}

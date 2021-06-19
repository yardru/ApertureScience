using ApertureScience;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace ApertureScience
{
    [ConfigurableRoute("Authentification")]
    public class AuthentificationController : Controller
    {
        private readonly EmployeesManager employees;

        public AuthentificationController(EmployeesManager employees, IWebHostEnvironment appEnvironment)
        {
            this.employees = employees;
        }

        [HttpPost]
        public IActionResult Authentification([FromForm] string email, [FromForm] string password)
        {
            var user = employees.GetAll().FirstOrDefault(emp => emp.Email == email && emp.Password == password);
            if (user == null)
                return BadRequest(new { errorText = "Invalid username or password." });

            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, $"{user.Id}"),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, $"{(int)user.Role}")
                };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "Token", 
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);


            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthentificationOptions.ISSUER,
                    audience: AuthentificationOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthentificationOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthentificationOptions.SECURITY_KEY, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                userId = user.Id,
                userRole = user.Role,
                accessToken = encodedJwt,
            };

            return Json(response);
        }
    }
}

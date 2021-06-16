using ApertureScience;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace ApertureScience
{
    [Route("api/[controller]")]
    public class AuthentificationController : Controller
    {
        private readonly EmployeesManager employees;

        public AuthentificationController(EmployeesManager context)
        {
            employees = context;
        }

        [HttpPost("/token")]
        public IActionResult Token([FromForm] string email, [FromForm] string password)
        {
            var identity = GetIdentity(email, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                userId = identity.Name,
                accessToken = encodedJwt,
            };

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string email, string password)
        {
            var user = employees.GetAll().FirstOrDefault(emp => emp.Email == email && emp.Password == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, $"{user.Id}"),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, $"{(int)user.Role}")
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}

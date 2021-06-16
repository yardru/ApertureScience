using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApertureScience
{
    [Authorize]
    abstract public class AuthorizedBaseController : ControllerBase
    {
        public int UserId
        {
            get
            {
                return int.Parse(User.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).Value);
            }
        }
        public Employee.Roles UserRole
        {
            get
            {
                return (Employee.Roles)int.Parse(User.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value);
            }
        }
    }
}

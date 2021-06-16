using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApertureScience
{
    /// <summary>
    ///     COOL BUT USELESS
    /// </summary>

    public class EmployeeAuthorizationRequirement : OperationAuthorizationRequirement
    {
        public delegate bool Authorization(int userId, Employee.Roles userRole, Employee employee);
        public Authorization AuthorizationRequirement;
    }

    public class EmployeeAuthorization: AuthorizationHandler<EmployeeAuthorizationRequirement, Employee>
    {
        public static EmployeeAuthorizationRequirement Create =
            new EmployeeAuthorizationRequirement { Name = nameof(Create),
                AuthorizationRequirement = (userId, userRole, employee) => userRole > employee.Role
            };
        public static EmployeeAuthorizationRequirement Read =
            new EmployeeAuthorizationRequirement { Name = nameof(Read),
                AuthorizationRequirement = (userId, userRole, employee) => true
            };
        public static EmployeeAuthorizationRequirement UpdateFrom =
            new EmployeeAuthorizationRequirement { Name = nameof(UpdateFrom),
                AuthorizationRequirement = (userId, userRole, employee) => userId == employee.Id || userRole > employee.Role
            };
        public static EmployeeAuthorizationRequirement UpdateTo =
            new EmployeeAuthorizationRequirement { Name = nameof(UpdateTo),
                AuthorizationRequirement = (userId, userRole, employee) => userId == employee.Id ? userRole == employee.Role : userRole >= employee.Role
            };
        public static EmployeeAuthorizationRequirement Delete =
            new EmployeeAuthorizationRequirement { Name = nameof(Delete),
                AuthorizationRequirement = (userId, userRole, employee) => userRole > employee.Role
            };

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployeeAuthorizationRequirement requirement, Employee employee)
        {
            var userId = int.Parse(context.User.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).Value);
            var userRole = (Employee.Roles)int.Parse(context.User.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value);

            if (requirement.AuthorizationRequirement(userId, userRole, employee))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}

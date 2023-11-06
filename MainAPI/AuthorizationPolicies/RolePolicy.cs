using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MainAPI.AuthorizationPolicies
{
    public class RolePolicy : IAuthorizationRequirement
    {
        public int Role { get; }

        public RolePolicy(int role)
        {
            Role = role;
        }
    }
    public class RolePolicyHandler : AuthorizationHandler<RolePolicy>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolePolicy requirement)
        {
            var userRole = context.User.FindFirstValue(ClaimTypes.Role);

            if (userRole != null && int.TryParse(userRole, out int userRoleLevel))
            {
                if (userRoleLevel >= requirement.Role)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}

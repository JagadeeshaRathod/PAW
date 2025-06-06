using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace swas.BAL.Utility
{
    public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
        {
            if (context.User.IsInRole("StakeHolders") || context.User.IsInRole("Admin") || context.User.IsInRole("Unit"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

    }

}


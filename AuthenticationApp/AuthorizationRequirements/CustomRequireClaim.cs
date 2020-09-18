using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApp.AuthorizationRequirements
{
    public class CustomRequireClaim : IAuthorizationRequirement
    {
        public CustomRequireClaim(string claimType)
        {
            _claimType = claimType;
        }

        public string _claimType { get; }
    }

    public class CustomRequireClaimHandler : AuthorizationHandler<CustomRequireClaim>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequireClaim requirement)
        {
            var hasClaim = context.User.Claims.Any(c => c.Type == requirement._claimType);
            if (hasClaim)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireCustomClaim(
            this AuthorizationPolicyBuilder builder,
            string claimType)
        {
            builder.AddRequirements(new CustomRequireClaim(claimType));
            return builder;
        }
    }
}

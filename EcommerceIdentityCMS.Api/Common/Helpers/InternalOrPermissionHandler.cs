using EcommerceIdentityCMS.Api.Common.Requirement;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceIdentityCMS.Api.Common.Helpers
{
    public class InternalOrPermissionHandler : AuthorizationHandler<InternalOrPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, InternalOrPermissionRequirement requirement)
        {
            // 1. Nếu là Token hệ thống từ Gateway (Có scope user.internal) -> CHO QUA LUÔN
            if (context.User.HasClaim(c => c.Type == "scope" && c.Value == "user.internal"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}

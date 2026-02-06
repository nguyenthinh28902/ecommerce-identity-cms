using EcommerceIdentityCMS.Api.Common.Requirement;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceIdentityCMS.Api.Common.Helpers
{
    public class InternalOrPermissionHandler : AuthorizationHandler<InternalOrPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, InternalOrPermissionRequirement requirement)
        {
            // 1. Kiểm tra nếu có Scope "user.internal" -> Đây là System Token (người nhà)
            // Cho phép truy cập TẤT CẢ các API có gắn InternalOrPermissionRequirement
            if (context.User.HasClaim(c => c.Type == "scope" && c.Value == "user.internal"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // 2. Nếu không phải hệ thống, kiểm tra quyền (Permission) của User (User Token)
            // Giả sử quyền của user được lưu trong Claim tên là "permission" hoặc "scope" tương ứng
            var userPermissions = context.User.FindAll(c => c.Type == "scope").Select(c => c.Value);

            if (userPermissions.Contains(requirement.RequiredPermission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

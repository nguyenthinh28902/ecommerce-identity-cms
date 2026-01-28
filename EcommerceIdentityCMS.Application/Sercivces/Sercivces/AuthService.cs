using EcommerceIdentityCMS.Application.Sercivces.Interfaces;
using EcommerceIdentityCMS.Core.DTOs.Account;
using EcommerceIdentityCMS.Core.Entities;
using EcommerceIdentityCMS.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// Để dùng .Include() và .ThenInclude()
using System.Linq; // Để dùng các hàm mở rộng của IQueryable

namespace EcommerceIdentityCMS.Application.Sercivces.Sercivces
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationDepartment> _roleManager;

        public AuthService(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationDepartment> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<SignInResponseDto?> ValidateUserAsync(SignInRequestDto signInRequestDto)
        {
            var user = await _userManager.Users.AsNoTracking().Include(x => x.Workplace)
                .Include(x => x.UserDepartments).ThenInclude(x => x.Department).ThenInclude(x => x.Permissions)
                .FirstOrDefaultAsync(x => x.Id == signInRequestDto.Id);
            if (user != null && await _userManager.CheckPasswordAsync(user, signInRequestDto.Password))
            {
                if(!user.IsActive) throw new UnauthorizedException("Tài khoản không hoạt động");
                var userDepartments = user.UserDepartments.Select(x => x.Department.DeptCode.ToString()).ToList();
                var departmentPermissions = user.UserDepartments.SelectMany(x => x.Department.Permissions).ToList();
                var scopes = BuildScopes(departmentPermissions);
                var workplaceId = user.WorkplaceId;

                return new SignInResponseDto { Id = user.Id, Email = user.Email ?? string.Empty, Roles = userDepartments, WorkplaceId = workplaceId, Scopes = scopes };
            }
            return null;
        }

        private List<string> BuildScopes(IEnumerable<DepartmentPermission> departmentPermissions)
        {
            var scopes = new HashSet<string>();

            foreach (var p in departmentPermissions)
            {
                if (string.IsNullOrWhiteSpace(p.FunctionCode))
                    continue;

                var function = p.FunctionCode.ToLowerInvariant();

                if (p.CanRead)
                    scopes.Add($"{function}.read");

                if (p.CanCreate || p.CanUpdate || p.CanDelete)
                    scopes.Add($"{function}.write");

            }

            return scopes.ToList();
        }

    }
}

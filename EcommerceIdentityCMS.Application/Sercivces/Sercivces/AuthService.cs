using EcommerceIdentityCMS.Application.Sercivces.Interfaces;
using EcommerceIdentityCMS.Core.DTOs.Account;
using EcommerceIdentityCMS.Core.Entities;
using EcommerceIdentityCMS.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcommerceIdentityCMS.Application.Sercivces.Sercivces
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationDepartment> _roleManager;
        private readonly ICurrentUserService _currentUserService;
        public AuthService(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationDepartment> roleManager,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _currentUserService = currentUserService;
        }

        public async Task<SignInResponseDto?> ValidateUserAsync(SignInRequestDto signInRequestDto)
        {
            var user = await _userManager.Users.AsNoTracking().Include(x => x.Workplace)
                .Include(x => x.UserDepartments).ThenInclude(x => x.Department).ThenInclude(x => x.Permissions)
                .FirstOrDefaultAsync(x => x.Id == signInRequestDto.Id);
            if (user != null && await _userManager.CheckPasswordAsync(user, signInRequestDto.Password))
            {
                if(!user.IsActive) throw new UnauthorizedException("Tài khoản không hoạt động");
                var signInResponseDto = await GetSignInResponseDto(user);
                return signInResponseDto;
            }
            return null;
        }

        public async Task<SignInResponseDto?> GetSignInResponseDto()
        {
            var userId = _currentUserService.UserId;
            var user = await _userManager.Users.AsNoTracking().Include(x => x.Workplace)
              .Include(x => x.UserDepartments).ThenInclude(x => x.Department).ThenInclude(x => x.Permissions)
              .FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) return null;
            var signInResponseDto = await GetSignInResponseDto(user);
            return signInResponseDto;
        }

        private async Task<SignInResponseDto?> GetSignInResponseDto(ApplicationUser user)
        {
            var userDepartments = user.UserDepartments.Select(x => x.Department.DeptCode.ToString()).ToList();
            var departmentPermissions = user.UserDepartments.SelectMany(x => x.Department.Permissions).ToList();
            var scopes = BuildScopes(departmentPermissions);
            var workplaceId = user.WorkplaceId;

            return new SignInResponseDto { Id = user.Id, Email = user.Email ?? string.Empty, Roles = userDepartments, WorkplaceId = workplaceId, Scopes = scopes };
        }


        private List<string> BuildScopes(IEnumerable<DepartmentPermission> departmentPermissions)
        {
            var scopes = new HashSet<string>();

            foreach (var p in departmentPermissions)
            {
                if (string.IsNullOrWhiteSpace(p.FunctionCode))
                    continue;

                var function = p.FunctionCode.ToLowerInvariant();

                switch (p)
                {
                    case { CanRead: true }:
                        scopes.Add($"{function}.read");
                        break;
                    case { CanCreate: true }:
                        scopes.Add($"{function}.create");
                        break;
                    case { CanUpdate: true }:
                        scopes.Add($"{function}.update");
                        break;
                    case { CanDelete: true }:
                        scopes.Add($"{function}.delete");
                        break;
                    default:
                        break;
                }
            }

            return scopes.ToList();
        }

    }
}

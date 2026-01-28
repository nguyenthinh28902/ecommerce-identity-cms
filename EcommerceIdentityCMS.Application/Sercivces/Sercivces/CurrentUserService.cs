using EcommerceIdentityCMS.Application.Sercivces.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceIdentityCMS.Application.Sercivces.Sercivces
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CurrentUserService> _logger;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, ILogger<CurrentUserService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public int UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;

                // Kiểm tra cả 'sub' (chuẩn JWT) và 'NameIdentifier' (chuẩn .NET)
                var value = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation(user?.FindFirst(ClaimTypes.Role)?.Value);
                _logger.LogInformation(user?.FindFirst("wid")?.Value);
                _logger.LogInformation(user?.FindFirst(ClaimTypes.Email)?.Value);
                return int.TryParse(value, out var userId) ? userId : 0;
            }
        }

        public string? Email
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                // Kiểm tra cả 'sub' (chuẩn JWT) và 'NameIdentifier' (chuẩn .NET)
                var value = user?.FindFirst(JwtRegisteredClaimNames.Email)?.Value;          
                return value ?? string.Empty;
            }
        }

        public string? Role
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                // Kiểm tra cả 'sub' (chuẩn JWT) và 'NameIdentifier' (chuẩn .NET)
                var value = user?.FindFirst(ClaimTypes.Role)?.Value;
                return value ?? string.Empty;
            }
        }

        public int WorkplaceId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;

                // Kiểm tra cả 'sub' (chuẩn JWT) và 'NameIdentifier' (chuẩn .NET)
                var value = user?.FindFirst("wid")?.Value;
                          

                return int.TryParse(value, out var userId) ? userId : 0;
            }
        }
    }
}

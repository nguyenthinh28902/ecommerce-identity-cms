using EcommerceIdentityCMS.Application.Sercivces.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EcommerceIdentityCMS.Application.Sercivces.Sercivces
{

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpContext? HttpContext => _httpContextAccessor.HttpContext;

        public int UserId
        {
            get
            {
                // Đọc từ Header do Gateway truyền xuống
                var value = HttpContext?.Request.Headers["X-User-Id"].ToString();
                return int.TryParse(value, out var userId) ? userId : 0;
            }
        }

        // Email có thể không có trong Header trừ khi bạn bổ sung ở Gateway
        public string? Email => HttpContext?.Request.Headers["X-User-Email"].ToString() ?? string.Empty;

        public string? Role
        {
            get
            {
                // Đọc từ Header X-User-Roles (chuỗi cách nhau bằng dấu phẩy)
                // Lấy role đầu tiên hoặc xử lý tùy logic của bạn
                var roles = HttpContext?.Request.Headers["X-User-Roles"].ToString();
                return roles?.Split(',').FirstOrDefault() ?? string.Empty;
            }
        }

        public int WorkplaceId
        {
            get
            {
                // Đọc từ Header X-User-WorkplaceId
                var value = HttpContext?.Request.Headers["X-User-WorkplaceId"].ToString();
                return int.TryParse(value, out var wid) ? wid : 0;
            }
        }

        // Thêm hàm này nếu bạn muốn lấy toàn bộ danh sách Roles
        public List<string> Roles =>
            HttpContext?.Request.Headers["X-User-Roles"].ToString()
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .ToList() ?? new List<string>();
    }
}

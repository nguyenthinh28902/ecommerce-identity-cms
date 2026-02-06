using EcommerceIdentityCMS.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace EcommerceIdentityCMS.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Cho phép request đi tiếp sang các Middleware khác hoặc Controller
                await _next(context);
            }
            catch (UnauthorizedException ex)
            {
                // Bắt lỗi 401 khi User không hợp lệ ở lớp Application
                _logger.LogWarning($"Xác thực thất bại: {ex.Message}");
                await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"Không tìm thấy tài nguyên: {ex.Message}");
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                // Bắt các lỗi hệ thống không mong muốn (500)
                _logger.LogError(ex, "Một lỗi hệ thống đã xảy ra.");
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                isSuccess = false,
                statusCode = (int)statusCode,
                error = exception.Message,
            };
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}

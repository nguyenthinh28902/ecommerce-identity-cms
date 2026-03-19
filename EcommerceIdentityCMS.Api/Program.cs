using EcommerceIdentityCMS.Api.Common.Helpers;
using EcommerceIdentityCMS.Api.DependencyInjection;
using EcommerceIdentityCMS.Api.Middlewares;
using EcommerceIdentityCMS.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGenConfiguration(builder.Configuration);
//add DI 
builder.Services.AddApplicationDI(builder.Configuration);


//tránh ánh xạ token
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthenticationExtensions(builder.Configuration);

builder.Services.AddServiceDI(builder.Configuration);
var app = builder.Build();

// Đặt ở đây để bắt được lỗi của mọi Middleware phía sau
app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.DisplayRequestDuration());
}
app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseAuthentication(); // Phải có dòng này và nằm trên UseAuthorization
app.UseAuthorization();

app.MapControllers();
await app.Services.SeedDatabaseAsync();
app.Run();

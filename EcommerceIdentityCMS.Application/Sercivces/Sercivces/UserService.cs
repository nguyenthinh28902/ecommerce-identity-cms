using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceIdentityCMS.Application.Sercivces.Interfaces;
using EcommerceIdentityCMS.Core.DTOs.ApplicationUser;
using EcommerceIdentityCMS.Core.Entities;
using EcommerceIdentityCMS.Core.Exceptions;
using EcommerceIdentityCMS.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcommerceIdentityCMS.Application.Sercivces.Sercivces
{
    public class UserService : IUserService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public UserService(UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }


        public async Task<Result<UserInforDto>> GetUserInfoAsync()
        {
            var userId = _currentUserService.UserId;
            var userInfo = await _userManager.Users.AsNoTracking()
                          .Where(u => u.Id == userId)
                          .ProjectTo<UserInforDto>(_mapper.ConfigurationProvider)
                          .FirstOrDefaultAsync();
            if (userInfo == null)
            {
                throw new UnauthorizedException("Tài khoản không tồn tại hoặc đã bị xóa.");
            }

            var result = Result<UserInforDto>.Success(userInfo, $"Thông tin nhân sự {userInfo.FullName}");

            return result;
        }
    }
}

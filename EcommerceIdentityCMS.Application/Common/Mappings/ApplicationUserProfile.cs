using AutoMapper;
using EcommerceIdentityCMS.Core.DTOs.ApplicationUser;
using EcommerceIdentityCMS.Core.Entities;

namespace EcommerceIdentityCMS.Application.Common.Mappings
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, UserInforDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))

                // 1. Lấy tên phòng ban chính (Primary)
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src =>
                    src.UserDepartments
                        .Where(ud => ud.IsPrimary)
                        .Select(ud => ud.Department.Name)
                        .FirstOrDefault() ?? string.Empty))

                // 2. Lấy danh sách mã tất cả phòng ban
                // Bỏ .ToList() để EF Core tự xử lý tối ưu hơn khi Select
                .ForMember(dest => dest.DeptCodes, opt => opt.MapFrom(src =>
                    src.UserDepartments.Select(ud => ud.Department.DeptCode.ToString())))

                // 3. Lấy thông tin Nơi làm việc (Sử dụng toán tử null-conditional ?. để an toàn)
                .ForMember(dest => dest.WorkplaceName, opt => opt.MapFrom(src =>
                    src.Workplace != null ? src.Workplace.Name : "Không xác định"));
        }
    }
}

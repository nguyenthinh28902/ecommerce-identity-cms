using AutoMapper;
using EcommerceIdentityCMS.Core.DTOs.ApplicationUser;
using EcommerceIdentityCMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceIdentityCMS.Application.Common.Mappings
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile() {

            CreateMap<ApplicationUser, UserInforDto>()
                         .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                         // Sử dụng logic hàm GetRole nhưng viết dưới dạng Expression để AutoMapper dịch sang SQL
                         .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src =>
                             src.UserDepartments.Where(x => x.IsPrimary == true).Select(ud => ud.Department.Name).FirstOrDefault() ?? string.Empty))
                         // Lấy thông tin Nơi làm việc
                         .ForMember(dest => dest.WorkplaceName, opt => opt.MapFrom(src =>
                             src.Workplace != null ? src.Workplace.Name : "Không xác định"));
        }
    }
}

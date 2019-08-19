using ApplicationCore.Entities;
using ApplicationCore.Helpers.Auth;
using ApplicationCore.Helpers.Pagination;
using AutoMapper;
using Infrastructure.IdentityData;
using Microsoft.AspNetCore.Authentication;
using Web.Models;
using Web.Models.Authentication;
using Web.Models.Category;

namespace Web.Configuration
{
    /// <summary>
    /// Automapper class configuration
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegistrationModel, Uploader>();
            CreateMap<LoginModel, Uploader>();
            CreateMap<RegistrationModel, GalleryUser>();
            CreateMap<IAuthProperties, AuthenticationProperties>();

            CreateMap<Category, CategoryModel>();
            CreateMap<IPaginationModel<Category>, CategoryViewModel>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Pagination, opt => opt.MapFrom(
                    src =>  new PaginationsProperties
                    {
                        CurrentPage = src.CurrentPage,
                        Pages = src.Pages,
                        TotalItems = src.TotalItems,
                        TotalPages = src.TotalPages
                    })
                );

        }
    }
}
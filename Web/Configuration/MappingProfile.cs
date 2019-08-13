using ApplicationCore.Entities;
using ApplicationCore.Helpers.Auth;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.IdentityData;
using Microsoft.AspNetCore.Authentication;
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
            CreateMap<Category, CategoryModel>();
            CreateMap<RegistrationModel, GalleryUser>();
            CreateMap<IAuthProperties, AuthenticationProperties>();
        }
    }
}
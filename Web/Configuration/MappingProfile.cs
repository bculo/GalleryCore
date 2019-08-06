using ApplicationCore.Entities;
using AutoMapper;
using Infrastructure.IdentityData;
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
        }
    }
}
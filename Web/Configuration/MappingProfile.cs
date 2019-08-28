using ApplicationCore.Entities;
using ApplicationCore.Helpers.Auth;
using ApplicationCore.Helpers.Pagination;
using AutoMapper;
using Infrastructure.IdentityData;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using Web.Models;
using Web.Models.Authentication;
using Web.Models.Category;
using Web.Models.Image;

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

            //Category mapper section
            CreateMap<Category, CategoryModel>();
            CreateMap<Category, EditCategoryModel>();
            CreateMap<Category, DeleteCategoryModel>();
            CreateMap<IPaginationModel<Category>, CategoryViewModel>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Pagination, opt => opt.MapFrom(src =>  new PaginationsProperties
                {
                    CurrentPage = src.CurrentPage,
                    Pages = src.Pages,
                    TotalItems = src.TotalItems,
                    TotalPages = src.TotalPages
                }));

            //Image mapper section
            CreateMap<Image, ImageBasicModel>();
            CreateMap<IPaginationModel<Image>, ImageDisplayModel>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Pagination, opt => opt.MapFrom(src => new PaginationsProperties
                {
                    CurrentPage = src.CurrentPage,
                    Pages = src.Pages,
                    TotalItems = src.TotalItems,
                    TotalPages = src.TotalPages
                }));
            CreateMap<Image, ImageRichModel>()
                .ForMember(dest => dest.UploderName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(tag => tag.Description).ToList()))
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => (src.Likes.Count(item => item.Liked))))
                .ForMember(dest => dest.Dislikes, opt => opt.MapFrom(src => (src.Likes.Count(item => !item.Liked))))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

            //Comment mapper section
            CreateMap<IPaginationModel<Comment>, CommentViewModel>()
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Pagination, opt => opt.MapFrom(src => new PaginationsProperties
                {
                    CurrentPage = src.CurrentPage,
                    Pages = src.Pages,
                    TotalItems = src.TotalItems,
                    TotalPages = src.TotalPages
                }));
            CreateMap<Comment, ImageComment>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
        }
    }
}
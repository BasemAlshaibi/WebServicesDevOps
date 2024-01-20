using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{


    public class AutoMapperProfiles : Profile
    {
 
        private readonly IConfiguration _config;


        public AutoMapperProfiles(IConfiguration config)
        {

            _config = config; // _config["ApiUrl"]


            CreateMap<AppUser, UserResponseDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.UserPhotos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.UserRoles.FirstOrDefault(x => x.UserId == src.Id).Role.Name));



            CreateMap<UserPhoto, UserPhotoDto>().ReverseMap();// خاصة بصور المستخدمين 
            CreateMap<UserUpdateDto, AppUser>();



            CreateMap<RegisterDto, AppUser>();

            CreateMap<NewsUpdateDto, News>();
            CreateMap<NewsAddDto, News>()
                        .ForMember(d => d.NewsImages, o => o.MapFrom(s => s.newsImageAdd));


            CreateMap<News, NewsRespDashboardDto>()
            .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.Category.Id))
             .ForMember(d => d.CreatedUserName, o => o.MapFrom(s => s.CreatedUser.KnownAs))
            .ForMember(d => d.UpdatedUserName, o => o.MapFrom(s => s.UpdatedUser.KnownAs))
            .ForMember(dest => dest.PhotoUrlThumbnail, opt => opt.MapFrom(src => _config["ApiUrl"] +
                  src.NewsImages.FirstOrDefault(x => x.IsMain).Folder + "Thumbnail_" + src.NewsImages.FirstOrDefault(x => x.IsMain).Id + ".jpg"));

            CreateMap<Category, CategoryDashboardDto>()
            .ForMember(dest => dest.NameCategoryUrl, opt => opt.MapFrom(src =>src.Name.Replace(" ", "_"))).ReverseMap();



    
 

            CreateMap<NewsImage, NewsImagesDto>()
           .ForMember(dest => dest.OriginalUrl, opt => opt.MapFrom(src =>
               _config["ApiUrl"] + src.Folder + "Original_" + src.Id + ".jpg"))
        .ForMember(dest => dest.FullscreenUrl, opt => opt.MapFrom(src =>
          _config["ApiUrl"] + src.Folder + "Fullscreen_" + src.Id + ".jpg"))
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src =>
             _config["ApiUrl"] + src.Folder + "Thumbnail_" + src.Id + ".jpg"));

 


 
 


            /* جلب الاقسام مع عدد نت اخبار كل قسم لواجهة الزوار العامة*/

            CreateMap<Category, CategoryVistorsDto>()
                .ForMember(dest => dest.NameCategoryUrl, opt => opt.MapFrom(src => src.Name.Replace(" ", "_")))
                .ForMember(d => d.News, o => o.MapFrom(s => s.News.Where(n => n.Status == Enum.Parse<PublishingStatus>("Published")).OrderByDescending(x => x.PublishedAt).Take(4)));

            /* جلب  بعض حقول الاخبار لواجهة الزوار العامة*/
            CreateMap<News, NewsRespHomeVistorsDto>()
            .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
            .ForMember(dest => dest.PhotoUrlThumbnail, opt => opt.MapFrom(src => _config["ApiUrl"] +  
                    src.NewsImages.FirstOrDefault(x => x.IsMain).Folder + "Thumbnail_" + src.NewsImages.FirstOrDefault(x => x.IsMain).Id + ".jpg"))
            .ForMember(dest => dest.PhotoUrlFullscreen, opt => opt.MapFrom(src => _config["ApiUrl"] +  
                    src.NewsImages.FirstOrDefault(x => x.IsMain).Folder + "Fullscreen_" + src.NewsImages.FirstOrDefault(x => x.IsMain).Id + ".jpg"));

            /* جلب بعض حقول الاخبار لصفحة التفاصيل بقسم الزوار العام*/
            CreateMap<News, NewsRespDetailsVistorsDto>()
                       .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
                       .ForMember(dest => dest.PhotoUrlFullscreen, opt => opt.MapFrom(src => _config["ApiUrl"] +  
                               src.NewsImages.FirstOrDefault(x => x.IsMain).Folder + "Fullscreen_" + src.NewsImages.FirstOrDefault(x => x.IsMain).Id + ".jpg"));

  

        
 

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Errors;
using API.Extensions;
using API.Helpers;
using API.Helpers.Parameters;
using API.Helpers.Specifications;
using API.Interfaces;
using AutoMapper;
 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace API.Controllers
{

    public class NewsController : BaseApiController
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IImageUploaderService _imageUploaderService;
 
        public NewsController(IUnitOfWork unitOfWork, IMapper mapper, IImageUploaderService imageUploaderService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageUploaderService = imageUploaderService;
        }



       [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsRespDashboardDto>>> GetNews([FromQuery] NewsSpecParams newsSpecParams)
        {

            var spec = new NewsSpecification(newsSpecParams);

            var news = await _unitOfWork.NewsDashboard.GetAllWithSpec(spec);

          //   if (news.Count <= 0 ) return NotFound(new ApiResponse(404));

            Response.AddPaginationHeader(news.CurrentPage, news.PageSize, news.TotalCount, news.TotalPages);

            return Ok(news);


        }

        [HttpGet]
        [Route("GetNewsWithSpecWithPagingToVistors")]  
        public async Task<ActionResult<IEnumerable<NewsRespHomeVistorsDto>>> GetNewsWithSpecWithPagingToVistors([FromQuery] NewsSpecParams newsSpecParams)
        {
            var spec = new NewsSpecification(newsSpecParams);

            var news = await _unitOfWork.NewsHomeVistors.GetAllWithSpec(spec);

/*
بعد تنفيذ الامر سنشيك على البرميتر المرسل لو كان فيه بحث فهنا سنغير المرتجع على النحو التالي
{
    "statusCode": 0,
    "message": null
}
بحيث انه ما يحولنا الى صفحة 404 وانما يظهر رسالة بان اللي يبحث عنه غير موجود\
وهذا سنضبطه بالانترسيتور في الانجولار
*/
             if (news.Count <= 0 && newsSpecParams.Search is not null) return NotFound(new ApiResponse());
/*
مالم لو العملية تنقل عادي بين الاخبار  فهنا لو مافيش نرجعه بالكود 404 من اجل يحوله بالانجولار لصفحة 
NotFound
*/

            if (news.Count <= 0 ) return NotFound(new ApiResponse(404));

            Response.AddPaginationHeader(news.CurrentPage, news.PageSize, news.TotalCount, news.TotalPages);

            return Ok(news);

        }

        [HttpGet]
        [Route("GetNewsWithSpecWithoutPaging")]  
        public async Task<ActionResult<IEnumerable<NewsRespHomeVistorsDto>>> GetNewsWithSpecWithoutPaging([FromQuery] NewsSpecParams newsSpecParams)
        {
            var spec = new NewsSpecification(newsSpecParams);

            var news = await _unitOfWork.NewsHomeVistors.GetAllWithSpecWithoutPaging(spec);

             if (news.Count <= 0 ) return NotFound(new ApiResponse(404));
 
            return Ok(news);

        }


     //   [ServiceFilter(typeof(NoOfReadIncrement))]
        [HttpGet]
       // [Authorize]
        [Route("{Id}")]
        public async Task<ActionResult<NewsRespDashboardDto>> GetNewsById(int Id)
        {
            var spec = new NewsSpecification(Id);
            var news = await _unitOfWork.NewsDashboard.GetEntityWithSpec(spec);
            if (news  is null ) return NotFound(new ApiResponse(404));
            return Ok(news);
        }
 
      //  [ServiceFilter(typeof(NoOfReadIncrement))]
        [HttpGet]
        [Route("GetNewsDetailsForVisitorsById/{Id}")]
        public async Task<ActionResult<NewsRespDetailsVistorsDto>> GetNewsDetailsForVisitorsById(int Id)
        {
            var spec = new NewsSpecification(Id);
            var news = await _unitOfWork.NewsDetailsVistors.GetEntityWithSpec(spec);
            if (news  is null ) return NotFound(new ApiResponse(404));
            return Ok(news);
        }

        [Authorize]
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> AddNews(NewsAddDto newsAddDto)
        {

            var news = _mapper.Map<News>(newsAddDto);

            news.CreatedUserId = User.GetUserId();

            news.CreatedAt = DateTime.UtcNow;

      

            await _unitOfWork.NewsDashboard.AddAsync(news);

            if (await _unitOfWork.Complete())
            {
                

                return NoContent();


            }


            return BadRequest(new ApiResponse(400, "Failed to Add news"));
        }


       // [Authorize]
        [HttpPut]
        [Route("Update/{Id}")]
        public async Task<ActionResult> UpdateNews(int Id, NewsUpdateDto newsUpdateDto)
        {

 

            News news = await _unitOfWork.NewsDashboard.GetByIdAsync(Id);

            if (news is null) return NotFound(new ApiResponse(404));

            news.UpdatedAt = DateTime.UtcNow;

            news.UpdatedUserId = User.GetUserId();

            var CurrentStatus = ((PublishingStatus)news.Status).ToString();

            if (newsUpdateDto.Status == "Published" && (CurrentStatus == "Scheduled" || CurrentStatus == "Pending" || CurrentStatus == "Draft"))
            {
                newsUpdateDto.PublishedAt = DateTime.UtcNow;
            }

            _mapper.Map(newsUpdateDto, news);

            _unitOfWork.NewsDashboard.Update(news);

            if (await _unitOfWork.Complete())
            {


                return NoContent();

            }

            return BadRequest(new ApiResponse(400, "Failed to update news"));
        }



        //News photos End Points
       // [Authorize]
        [HttpPost]
        [RequestSizeLimit(2 * 1024 * 1024)]// مسموح لحد اثنين ميجا يرتفع بالمرة الواحدة
        [Route("UploadImages/{newsId?}")]
        // [Route("UploadImages")] // IFormFile file
        public async Task<ActionResult<NewsImage>> UploadImages(int? newsId, IFormFile[] images)
        {
            // الكود التالي لن يكون له اثر لان الصور تاتي صورة كل مرة وليس مصفوفة من الصور
            if (images.Length > 10)
            {
                return BadRequest(new ApiResponse(400, "You Can't Upload more 10 images"));
            }

            var addedImageInfo = await _imageUploaderService.Process(images.Select(i => new ImageInputDto
            {
                Name = i.FileName,// لن نستخدمها في دالة البورسس في السيرفس
                Type = i.ContentType,// لن نستخدمها في دالة البورسيس في السيرفس
                Content = i.OpenReadStream()
            }));



            var newsImages = addedImageInfo.Select(i => new NewsImage
            {
                Id = i.Id,
                Folder = i.Folder,
                IsMain = false, //!newsId.HasValue && addedImageInfo.IndexOf(i) == 0 ? true : false,
                                //  IsMain =  newsId.HasValue && !news.NewsImages.Any() ? true : false,
                NewsId = newsId.HasValue ? newsId : null
            });


            // معناته ان هناك خبر معين موجود مسبقا نريد رفع الصورة اليه
            if (newsId.HasValue)
            {

                var spec = new NewsSpecification(newsId.Value);
                var news = await _unitOfWork.NewsDashboard.GetEntityWithSpecWithOutMap(spec);


                // في حالة رفع الصورة اثناء التعديل وهذه الصورة هي اول صورة فهنا نعمل انها الرئيسية
                if (!news.NewsImages.Any())
                {

                    newsImages = addedImageInfo.Select(i => new NewsImage
                    {
                        Id = i.Id,
                        Folder = i.Folder,
                        IsMain = true,
                        NewsId = newsId.HasValue ? newsId : null
                     });

                }

                await _unitOfWork.NewsImage.AddRangeAsync(newsImages);
                if (await _unitOfWork.Complete()) return Ok(newsImages);
                return BadRequest(new ApiResponse(400, "Problem addding photo"));

            }

     

            return Ok(newsImages);



        }
       // [Authorize]
        [HttpPut("set-main-photo/{newsId}/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int newsId, Guid photoId)
        {
            var spec = new NewsSpecification(newsId);
            var news = await _unitOfWork.NewsDashboard.GetEntityWithSpecWithOutMap(spec);

            var photo = news.NewsImages.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest(new ApiResponse(400, "This is already main photo"));

            var currentMain = news.NewsImages.FirstOrDefault(x => x.IsMain);

            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest(new ApiResponse(400, "Failed to set main photo"));
        }

       // [Authorize]
        [HttpDelete("delete-photo/{photoId}/{folder}/{newsId?}")]
        public async Task<ActionResult> DeletePhoto(Guid photoId, string folder, int? newsId)
        {

            if (newsId.HasValue)
            {
                // هنا حذف صورة من الداتا بيز ومن مساحة التخزين كما

                var spec = new NewsSpecification(newsId.Value);

                var news = await _unitOfWork.NewsDashboard.GetEntityWithSpecWithOutMap(spec);


                var photo = news.NewsImages.FirstOrDefault(x => x.Id == photoId);

                if (photo == null) return NotFound();

                if (photo.IsMain) return BadRequest(new ApiResponse(400, "You cannot delete news main photo"));

                string[] imagesToDeleted = new string[] { $"Fullscreen_{photo.Id}.jpg", $"Original_{photo.Id}.jpg", $"Thumbnail_{photo.Id}.jpg" };

                var storagePath = Path.Combine(Directory.GetCurrentDirectory(), $"Content{photo.Folder}".Replace("/", "\\"));

                foreach (var image in imagesToDeleted)
                {
                    System.IO.File.Delete(Path.Combine(storagePath, image));
                }

                // news.ImageFiles.Remove(photo);

                _unitOfWork.NewsImage.Delete(photo);

                if (await _unitOfWork.Complete()) return Ok();

                return BadRequest("Failed to delete the photo");
            }
            else
            {
                // معناه ان الصورة لسا لم تحفظ بالداتا بيز فيكفي ان نحذفها من التخزين
                string[] imagesToDeleted = new string[] { $"Fullscreen_{photoId}.jpg", $"Original_{photoId}.jpg", $"Thumbnail_{photoId}.jpg" };

                byte[] data = Convert.FromBase64String(folder);
                string decodedfolderpath = Encoding.UTF8.GetString(data);

                var storagePath = Path.Combine(Directory.GetCurrentDirectory(), $"Content{decodedfolderpath}".Replace("/", "\\"));

                foreach (var image in imagesToDeleted)
                {
                    System.IO.File.Delete(Path.Combine(storagePath, image));
                }

                return Ok();
            }



        }


        // Categories End Points
 
        [HttpGet]
        [Route("~/api/GetCategoriesWithSpecWithoutPaging")]
        public async Task<ActionResult<IEnumerable<CategoryDashboardDto>>> GetCategoriesWithSpecWithoutPaging([FromQuery] CategoryParams categoryParams)
        {
 
            var spec = new CategorySpecification(categoryParams);

            var Categories = await _unitOfWork.CategoriesDashboard.GetAllWithSpecWithoutPaging(spec);

          //  if (Categories.Count <= 0 ) return NotFound(new ApiResponse(404));

             return Ok(Categories);

        }
        [HttpGet]
        [Route("~/api/GetCategoriesNewsWithSpec")]
        public async Task<ActionResult<IEnumerable<CategoryVistorsDto>>> GetCategoriesNewsWithSpec([FromQuery] CategoryParams categoryParams)
        {
 
            var spec = new CategorySpecification(categoryParams);

            var Categories = await _unitOfWork.CategoriesVistors.GetAllWithSpecWithoutPaging(spec);

         if (Categories.Count <= 0 ) return NotFound(new ApiResponse(404)); 
            return Ok(Categories);

        }
       // [Authorize]
        [HttpGet]
        [Route("~/api/Category/{Id}")]
        public async Task<ActionResult<CategoryDashboardDto>> GetCategoryByIdAsync(int Id)
        {
            var spec = new CategorySpecification(Id);
            var category = await _unitOfWork.CategoriesDashboard.GetEntityWithSpec(spec);
            if (category  is null ) return NotFound(new ApiResponse(404));
            return Ok(category);


        }

     
 
       // [Authorize]
        [HttpPut]
        [Route("~/api/Category/UpdateAll")]
        public async Task<ActionResult> UpdateCategories(IEnumerable<CategoryDashboardDto> categoryDto)
        {


            var categoryList = _mapper.Map<IEnumerable<Category>>(categoryDto);

            _unitOfWork.CategoriesDashboard.UpdateRange(categoryList);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest(new ApiResponse(400, "Failed to update category"));

        }


    }
}



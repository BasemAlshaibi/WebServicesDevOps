using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Errors;
using API.Extensions;
using API.Helpers;
using API.Helpers.Specifications;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
   // [Authorize]
    public class UsersController : BaseApiController
    {

        private readonly IMapper _mapper;
        private readonly IPhotoCloudinaryService _PhotoCloudinaryService;
        private readonly IUnitOfWork _unitOfWork;
        public UsersController(IUnitOfWork unitOfWork, IMapper mapper,
            IPhotoCloudinaryService PhotoCloudinaryService )
        {
            _unitOfWork = unitOfWork;
            _PhotoCloudinaryService = PhotoCloudinaryService ;
            _mapper = mapper;
        }

      [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers([FromQuery] UserSpecParams userSpecParams)
        {


            var spec = new UserSpecification(userSpecParams);

            var users = await _unitOfWork.Users.GetAllWithSpec(spec);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);

        }
       [Authorize]
       [HttpGet("{id:int}", Name = "GetUserById")] 
        public async Task<ActionResult<UserResponseDto>> GetUserById(int Id)
        {

            var spec = new UserSpecification(Id);
            return await _unitOfWork.Users.GetEntityWithSpec(spec);

        }
        [Authorize]
        [HttpGet]
        [Route("GetUserByName/{name}")]  
        public async Task<ActionResult<UserResponseDto>> GetUserByName(string name)
        {
            var spec = new UserSpecification(name);
            return await _unitOfWork.Users.GetEntityWithSpec(spec);
        }


      
   

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult> UpdateUser(UserUpdateDto memberUpdateDto)
        {

            var user = await _unitOfWork.Users.GetByIdAsync(User.GetUserId());

            _mapper.Map(memberUpdateDto, user);

            _unitOfWork.Users.Update(user);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest(new ApiResponse(400, "Failed to update user"));
        }

        // عملية الاضافة هنا مزدوجة بحيث نرفع الصورة للكلاود وبعدين نخزن عنوانها بسجل المستخدم بالداتا بيز
        [Authorize]
        [HttpPost("add-photo")]
        public async Task<ActionResult<UserPhotoDto>> AddPhoto(IFormFile file)// نستقبل ملف جاء عبر ريكوست اتش تي تي بي
        {
            var spec = new UserSpecification(User.GetUserId());
            var user = await _unitOfWork.Users.GetEntityWithSpecWithOutMap(spec);

            // الان نستدعي دالة رفع الصورة للكلاود ونحزن الناتج
            var result = await _PhotoCloudinaryService.AddPhotoToCloudinaryAsync(file);
            // نفح الناتج لو فيه خطا يعيد نصه
            if (result.Error != null) return BadRequest(new ApiResponse(400, result.Error.Message));
            // مالم فهنا نبدا باضافة الصورة كمان الى الداتا بيز للمستخدم المناسب لها

            // نجهز اوبجكت من كيان الصورة بحيث ناخذ من المرتجع من الكلاود رابطا والبلك آي دي
            var photo = new UserPhoto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            // الان نتحقق اذا لم يكن المستخدم يملك صور من سابق مرتبطه به فان الصورة اللي انضافت تكون تلقائيا هي الرئيسية
            if (user.UserPhotos.Count == 0)
            {
                photo.IsMain = true;
            }
            // نقوم بعملية الاضافة للداتا بيز
            user.UserPhotos.Add(photo);
            // نحفظ التغيرات فاذا رجعت انه ترو اي تم الامر فهنا نحضر الاستجابة للفورنت اند
            if (await _unitOfWork.Complete())
            {
                // هنا دالة حالة 201 والتي تعيد لنا رابط الريسورس للمستخدم بالهدر
                // نمرر لها ثلاث برميترات الاولى اسم الراوت حق دالة جلب المستخدم بواسطة اسمه
                // والبرميتر الثاني هو اوبحكت يحمل الاي دي او  اللي ستستقبله دالة جلب المستخدم
                // والبيانات كاملة بعد عمل ماب لها لتضبيط شكلها وهي راجعة
                //لذلك نعدل قبل تعريف دالة جلب المستخدم اعلاه لتكون على النحو التالي
                /*
                  [HttpGet("{id:int}")]
                   [HttpGet("{id}", Name = "GetUserById")] 
                   public async Task<ActionResult<MemberResponseDto>> GetUserById(int id)
                */
                // ولو كنا نعمل بحث بالاسم
                /*   [HttpGet("{username}", Name = "GetUser")]
                     public async Task<ActionResult<MemberDto>> GetUser(string username)
                */

                return CreatedAtRoute("GetUserById", new { id = user.Id }, _mapper.Map<UserPhotoDto>(photo));
            }

            // في حالة انه السيف اوول ما رجعت ترو فنرجع باد ريكوست
            return BadRequest(new ApiResponse(400, "Problem addding photo"));
        }

        [Authorize]
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
           
         
        
            var spec = new UserSpecification(User.GetUserId());
            var user = await _unitOfWork.Users.GetEntityWithSpecWithOutMap(spec);

            var photo = user.UserPhotos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest(new ApiResponse(400, "This is already your main photo"));
            

            var currentMain = user.UserPhotos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _unitOfWork.Complete()) return NoContent();

        
            return BadRequest(new ApiResponse(400, "Failed to set main photo"));
        }
        [Authorize]
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
 
 
            var spec = new UserSpecification(User.GetUserId());
            var user = await _unitOfWork.Users.GetEntityWithSpecWithOutMap(spec);

            var photo = user.UserPhotos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest(new ApiResponse(400, "You cannot delete your main photo"));

            if (photo.PublicId != null)
            {
                var result = await _PhotoCloudinaryService.DeletePhotoFromCloudinaryAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.UserPhotos.Remove(photo);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest(new ApiResponse(400, "Failed to delete the photo"));
        }
    }
}
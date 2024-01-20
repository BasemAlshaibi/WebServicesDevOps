using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        // نقوم بحقن عدة سيرفسس واهمها الخاصة باليوزر والرول والاساين منجر
        // وهذه سنستخدمها بدلا عن الداتا كونتكست في الوصول للجداول والتعديل عليها
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

         private readonly RoleManager<AppRole> _rolerManager;

        private readonly SignInManager<AppUser> _signInManager;
        public AccountRepository(UserManager<AppUser> userManager, RoleManager<AppRole> rolerManager   ,SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _rolerManager = rolerManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }



        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // سنعمل اوتو ماب من الدتو الى بيانات كيان المستخدمين لنبدا نشتغل عليها
            var user = _mapper.Map<AppUser>(registerDto);
            // نحرص على ان حروف الايميل تكون صغيرة في الداتا بيز
            user.Email = registerDto.Email.ToLower();

            // نعمل اسم المستخدم الجزء الاول بالايميل وسيكون هناك حقل اخر للاسم المستعار

            user.UserName = registerDto.Email.Substring(0, registerDto.Email.IndexOf('@')).ToLower();
            // ننشى الحساب الجديد

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            // اذا لم ينجح الامر نعيد الاخطاء للكلاينت
            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)errors += $"{error.Description},";

                return new AuthResponseDto { Message = errors };
            }

            if (registerDto.Role == null) await _userManager.AddToRoleAsync(user, "Author");

           var roleResult = await _userManager.AddToRoleAsync(user, registerDto.Role);

                // اذا لم ينجح الامر نعيد برضه رسالة الخطا للكلاينت
                if (!roleResult.Succeeded)
                {
                    var errors = string.Empty;

                    foreach (var error in result.Errors)
                        errors += $"{error.Description},";

                    return new AuthResponseDto { Message = errors };
                }

// لا نحتاج لارجاع الاوبجكت بالمعلومات فيه لان الذي يعمل الريجستر هو الادمن وليس الشخص نفسه
// وبالتالي يمكن نرجع رسالة فقط بان الحساب تم انشاءه بنجاح 
            return new AuthResponseDto
            {
                Email = user.Email,
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };


        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            //نتأكد اولا انه في مستخدم بنفس الاسم بعد جعله حروف مصغرة
            var user = await _userManager.Users
           .Include(p => p.UserPhotos)
           .SingleOrDefaultAsync(x => x.Email.ToLower() == loginDto.Email.ToLower());

            if (user == null) return new AuthResponseDto { Message = "Invalid Email or Password" };

            // هنا نتحقق من الكلمة المرور علما ان البرميتر الثالث لو خليناه ترو فسيجعل له عدة محاولات ثم يعمل له بلوك لو اخطاء 
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);

            if (!result.Succeeded) return new AuthResponseDto { Message = "Invalid Email or Password" };

            return new AuthResponseDto
            {
                Email = user.Email,
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.UserPhotos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
              //  introduction =user.Introduction,
                Gender = user.Gender,
             /*   Photos = user.Photos.Select(p => new PhotoDto{
                    Id = p.Id,
                    Url = p.Url,
                    IsMain =p.IsMain                    
                } )*/
            };

        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());


            /*
                         if (await _userManager.FindByEmailAsync(model.Email) is not null)
                         return new ResponseAuthModel { Message = "Email is already registered!" };

                      if (await _userManager.FindByNameAsync(model.Username) is not null)
                      return new ResponseAuthModel { Message = "Username is already registered!" };
            */
        }

        public async Task<AuthResponseDto> GetCurrentUser(int Id)
        {
            // var user = await _userManager.FindByEmailFromClaimsPrinciple(User);

            var user = await _userManager.Users
            .Include(p => p.UserPhotos)
            .SingleOrDefaultAsync(x => x.Id == Id);
            
            return new AuthResponseDto
            {
                Email = user.Email,
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.UserPhotos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender,
               /* introduction =user.Introduction,
                Photos = user.Photos.Select(p => new PhotoDto{
                    Id = p.Id,
                    Url = p.Url,
                    IsMain =p.IsMain                    
                } )*/
            };

        }

        public async Task<AuthResponseDto> EditUserPermissions(string Id,  AdminEditUserDto adminEditUserDto)
        {
         
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null) return new AuthResponseDto { Message = "Could not find user" };
       
            user.Email = adminEditUserDto.Email.ToLower();

            user.UserName = adminEditUserDto.Email.Substring(0, adminEditUserDto.Email.IndexOf('@')).ToLower();

            user.KnownAs = adminEditUserDto.KnownAs;

             user.Gender = adminEditUserDto.Gender;

             user.Status = adminEditUserDto.Status;   

             var userRole = await _userManager.GetRolesAsync(user); 

             var result = await _userManager.RemoveFromRolesAsync(user, userRole );

            if (!result.Succeeded) return  new AuthResponseDto { Message = "Failed to remove old role" };  
 
             result = await _userManager.AddToRoleAsync(user, adminEditUserDto.Role);

             if (!result.Succeeded) return  new AuthResponseDto { Message = "Failed to add new role" };     
            
              await _userManager.UpdateAsync(user);

              return  new AuthResponseDto { Message = "Update secssfully" };  
  

        }
    }
}
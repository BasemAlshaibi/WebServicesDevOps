using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Errors;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController : BaseApiController
    {

        private readonly IAccountRepository _accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;

        }

     //   [Authorize]
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
        {
            // نبعث الاسم لدالة اخرى ننشىها بالاسفل لنتاكل ان اسم المستخدم ليس موجود
            if (await _accountRepository.CheckEmailExistsAsync(registerDto.Email)) return BadRequest( new ApiResponse(400, "this email is taken"));

            var result = await _accountRepository.RegisterAsync(registerDto);   

            if (result.Message is not null) return BadRequest(new ApiResponse(400,result.Message)  );

            return Ok(result);

 
        }

        [HttpPost("login")] // مؤقت الواحد
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var result = await _accountRepository.LoginAsync(loginDto); 

            if (result.Message is not null) return Unauthorized(new ApiResponse(result.Message)); //شكل المرتجع قد يكون{    "statusCode": 0,    "message": " Invalid Email or Password"}

            return Ok(result); 
                  
       }

        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<AuthResponseDto>> GetCurrentUser()
        {
            var result = await _accountRepository.GetCurrentUser(User.GetUserId()); 

           // if (result.Message is not null) return Unauthorized(new ApiResponse(401,result.Message)  );

            return Ok(result); 
        }


        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return Ok(await _accountRepository.CheckEmailExistsAsync(email));
        }
       
   // التعديلات الخاصة بالادمن على المستخدمين
   // AdminController

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPut("AdminController/{Id}")]
    public  async Task<ActionResult<AuthResponseDto>> EditUserPermissions(string Id, [FromBody] AdminEditUserDto adminEditUserDto)
    {

        var result = await _accountRepository.EditUserPermissions(Id,adminEditUserDto);
         return Ok(result);
   
    }
   


    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IAccountRepository
    {
        
      Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
      Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

 
      Task<bool> CheckEmailExistsAsync(string email);

      Task<AuthResponseDto> GetCurrentUser(int Id);

      Task<AuthResponseDto> EditUserPermissions(string Id,  AdminEditUserDto adminEditUserDto);
      


      
      
    }
}
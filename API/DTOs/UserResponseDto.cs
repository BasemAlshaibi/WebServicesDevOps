
using System;
using System.Collections.Generic;
using API.Entities;

namespace API.DTOs
{
    public class UserResponseDto
    {   
        
        public int Id { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }
        
        public string PhotoUrl { get; set; }
      
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }

         public bool Status { get; set; } 

       
        // هنا خلينا الكولكشن من نوع الفوتو دتو وبالتالي لا وجود لتداخل خصائص النفجيشن بين الكيانين 
        public ICollection<UserPhotoDto> UserPhotos { get; set; }

 
         public string Role { get; set; }

 
     
    }
    }
 
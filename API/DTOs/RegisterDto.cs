using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required] public string Email { get; set; }
        public string KnownAs { get; set; }
        [Required] public string Gender { get; set; }
 
        [Required]
     //   [StringlLength(8, MinimumLength = 4)]
        public string Password { get; set; }
      //  public List<string> Roles { get; set; }
 
         public string Role { get; set; }


 

    }
}
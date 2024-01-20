using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class  AuthResponseDto
    {

        public string Email { get; set; }

        public string UserName { get; set; }
        public string Token { get; set; }
        public string PhotoUrl { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }

        public string Message { get; set; } 
 //     public string introduction { get; set; }


    //   public IEnumerable<PhotoDto> Photos { get; set; } 

    }
}
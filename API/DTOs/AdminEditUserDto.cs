using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class AdminEditUserDto
    {
        public string Email { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }

        public bool Status { get; set; }

 

        
    }
}
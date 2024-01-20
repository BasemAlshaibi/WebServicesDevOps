using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserUpdateDto
    {
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
    }
}
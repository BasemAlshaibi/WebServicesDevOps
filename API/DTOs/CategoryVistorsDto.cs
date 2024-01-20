using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class CategoryVistorsDto
    {
         public int? Id { get; set; }

         public string Name { get; set; } 
        public ICollection<NewsRespHomeVistorsDto> News { get; set; }

        public string NameCategoryUrl { get; set; } // تحت التجربة
    }
}


       

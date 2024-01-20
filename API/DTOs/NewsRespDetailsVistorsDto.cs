using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class NewsRespDetailsVistorsDto:NewsRespHomeVistorsDto
    {
        
       public string Content { get; set; } 
       public ICollection<NewsImagesDto> NewsImages { get; set; }

      public string Source { get; set; }



    }
}
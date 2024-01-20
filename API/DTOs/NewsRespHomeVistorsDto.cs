using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{

    // الداتا المطلوبة في الصفحة الرئيسية من  اعمدة جدول الاخبار
    public class NewsRespHomeVistorsDto 
    {
        
         public int Id { get; set; }

        public string Title { get; set; }

        public DateTime PublishedAt { get; set; } 

         public string Category { get; set; }

        public string PhotoUrlThumbnail { get; set; }

        public string PhotoUrlFullscreen { get; set; }


        public int CategoryId { get; set; }

 
       // public ICollection<ImageFileDto> NewsImages { get; set; }

        // public string Content { get; set; }




        
    }
}
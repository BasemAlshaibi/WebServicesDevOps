using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class NewsImagesDto
    {

        // البيانات المرتجعة للايند يوزر
        public Guid Id { get; set; }

        public string OriginalUrl { get; set; }  

        public string FullscreenUrl { get; set; }        

        public string ThumbnailUrl { get; set; }  

        
        public bool IsMain { get; set; }        
        
    }
}
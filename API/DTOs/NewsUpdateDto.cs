using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class NewsUpdateDto
    {
        
 
        public string Title { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; } 

         public string Source { get; set; }
 
        public bool isShowInMain { get; set; }

        public bool isChooseEditor { get; set; }

        public bool IsBreakingOrImportant { get; set; }

       public int BreakingOrImportantDuration { get; set; }   

        public  string Status { get; set; } 
         
        public DateTime PublishedAt { get; set; } 
    
        public int CategoryId { get; set; }


    }
}
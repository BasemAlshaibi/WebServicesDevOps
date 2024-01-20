using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class NewsRespDashboardDto
    {
         public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; } 

        public string CreatedUserName { get; set; }

        public DateTime CreatedAt { get; set; } 

        public DateTime PublishedAt { get; set; } 

         public string UpdatedUserName { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Source { get; set; }
 
        public bool isShowInMain { get; set; }

        public bool isChooseEditor { get; set; }

        public bool IsBreakingOrImportant { get; set; }

        public  string Status { get; set; } 

      // public  int StatusId { get; set; }  // سيحذف لاحقا
        public string PhotoUrlThumbnail { get; set; }
 
        public ICollection<NewsImagesDto> NewsImages { get; set; }


        public string Category { get; set; }

        public int CategoryId { get; set; }


         
        public int NoOfRead { get; set; }

     


    }
}
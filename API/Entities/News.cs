using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public enum PublishingStatus
    {
        Published,
        Draft,
        Scheduled,
        Pending,
        Rejected
    }

    public class News
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Source { get; set; }


        public string Summary { get; set; }

        public string Content { get; set; }

        public bool isShowInMain { get; set; }

        public bool isChooseEditor { get; set; }

         public bool IsBreakingOrImportant { get; set; }
 

        // بيانات الاضافة
        public int CreatedUserId { get; set; }
        public AppUser CreatedUser { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime PublishedAt { get; set; }

        // بيانات التعديل

        public int? UpdatedUserId { get; set; }
        public AppUser UpdatedUser { get; set; }

        public DateTime? UpdatedAt { get; set; }

 
        public PublishingStatus Status { get; set; }

        public int NoOfRead { get; set; } = 0;

 

        public ICollection<NewsImage> NewsImages { get; set; }


        public Category Category { get; set; }
        public int CategoryId { get; set; }



    }

}
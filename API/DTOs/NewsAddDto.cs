using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class NewsAddDto
    {
//
        public string Title { get; set; }

        public string Summary { get; set; }

        public string Source { get; set; }

        public string Content { get; set; }
//
        public bool isShowInMain { get; set; }

        public bool isChooseEditor { get; set; }

        public bool IsBreakingOrImportant { get; set; }

        public int BreakingOrImportantDuration { get; set; }   

    //    
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        public  string Status { get; set; } = "Published";

        public ICollection<NewsImage> newsImageAdd { get; set; }

        public int? CategoryId { get; set; }

    }
}
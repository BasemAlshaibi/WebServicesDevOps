using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Helpers.Specifications
{
    public class NewsSpecParams : PaginationParams
    {
       public string Category { get; set; } // فلترة بالاسم حاليا بالداشبورد

        public int? CategoryId { get; set; } // يتم اعتماده للفلترة عموما
  
       public string CreatedBy { get; set; } // اعتماد الفلترة بناء على الاي دي للمنشئ بدل المستخدم

        public DateTime MinDate { get; set; } = DateTime.MinValue;

        public DateTime MaxDate { get; set; } = DateTime.MaxValue;

        public string Sort { get; set; }

        public int Top { get; set; }

         public bool? isShowInMain { get; set; } = null;

         public bool? isChooseEditor { get; set; }= null;

        public bool? isBreakingOrImportant { get; set; }= null;


 
 
        public  string Status { get; set; }     

        private string _search;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();  
        }
    }
}
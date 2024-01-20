using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace API.Entities
{

    /* هذه الجداول تختص بالصور للاخبار والكتاب وطبقنا فيها هنا الوراثة */
    [Table("ImagesBase")]
    public class ImageBase
    {
        public Guid Id { get; set; }
        public string Folder { get; set; }
        public bool IsMain { get; set; }



    }

    [Table("NewsImages")]
    public class NewsImage : ImageBase
    {
        public int? NewsId { get; set; }
 
        [NotMapped]
        private string _Url;   // هذه الخاصية ليست بالداتا بيز وانما نعيد فيها الجزء الثاني من لينك الصورة بعد عملية الاضافة من اجل عرضها في دالة رفع الصورة لاغير
        // بالفورنت اند سنضيف جزء الدومين  الاولي للينك

        [NotMapped]
        public string Url
        {
            get =>  Folder + "Fullscreen_" + Id + ".jpg";  
            set => _Url = value.ToLower();
        }


 

    }



  
    
}
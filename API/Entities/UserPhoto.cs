using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{

  /* هذا الجدول يخص جدول الصور الذي سيتم حفظها في الكلاود والتي تخص المستخدمين فقط*/
  [Table("UserPhotos")]
  public class UserPhoto
    {
         public int Id { get; set; }
        // الصورة سترفع للسيرفر وسيخزن فقط رابطها
        public string Url { get; set; }
        // للشخص صورة رئيسية واحدة فقط من الصور المحملة له
        public bool IsMain { get; set; }
        // هذا عمود لاحقا سنستخدمه لتجاوز مشكلة تخزين الصور بالسيرفر
        public string PublicId { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }

 
    }

}
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
/*
    public class AppUser : IdentityUser<int>
*/
    

  public class AppUser :IdentityUser<int> 
    {
        /*
        لعمل بروبرتي بسرعة نكتب
        prop
        ثم زر التاب او انتر
                public int MyProperty { get; set; }
        **/
                 
     //   public int Id { get; set; }
     //   public string UserName { get; set; }
   
       // اسم شهرة او بديل قد يختلف عن اسم المستخدم
        public string KnownAs { get; set; }
        // اعطينا قيم اولية للحقلين التالين هما في الوقت الحالي 
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; }
        public string Introduction { get; set; }
 
        public bool Status { get; set; }  = true; 
           
        // لكل مستخدم عدة صور قد يكون مرتبط بها
        // لذا هنا كولكشن من نوع كلاس صور 
        // سنقوم بانشاءه كذلك بملف منفصل في مجلد الاتنتيز
       public ICollection<UserPhoto> UserPhotos { get; set; }

  
      
       // خاصية خاصة بالنفجيشن الى جدول اليوزر رولز
      public ICollection<AppUserRole> UserRoles { get; set; }


        public ICollection<News> CreatedUsers { get; set; }
        public ICollection<News> UpdatedUsers { get; set; }


       

  


 
  }
}

 
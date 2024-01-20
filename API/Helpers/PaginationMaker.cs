using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PaginationMaker<T> : List<T> // يعمل وراثه من الكلاس الخاص باللست اي سيورث دالة 
    {
        // نعرف المتغيرات الاربعة التي سنعيدها للكلايت
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        /*
        لو حبينا نطور الكلاس فنقدر نضيف
          public bool HasPrevious => CurrentPage > 1;
          public bool HasNext => CurrentPage < TotalPages;

          //  HasPrevious is true if CurrentPage is larger than 1, 
           //     and HasNext is calculated if CurrentPage is smaller than the number of total pages
        */

        // دالة البناء تستقبل اربعة برميترات من دالة الانشاء التي في الاسفل
        public PaginationMaker(IEnumerable<T> items, int count, int pageIndex, int pageSize)
        {
            // هنا نساوي القيم التي ستاتي اثناء انشاء مثيل الكلاس 
            CurrentPage = pageIndex; // هنا نساوي مباشرة
            // علشان نطلع اجمالي عدد الصفحات سنحتاج لقسمة عدد السجلات بالجدول على حجم الصفحة اي على رقم العناصر بكل صفحة
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize; // مساواة مباشرة
            TotalCount = count; // مساواة مباشرة
            AddRange(items); // هنا دالة من دوال اللست لان الكلاس دا يورث من اللست اي اضفنا العناصر
        }



        // هذه الدالة هي ستاتك لذلك ستكون كاستنشن سنستخدمها بالريبو
        // ستستقبل السورس واللي يمثل كامل بيانات الكيات بالداتا بيز اضافة الى رقم الصفحة وحج العناصر بها
        // وهذه بيانات وردت من الكلاينت
        public static async Task<PaginationMaker<T>> CreateAsync(IQueryable<T> source, int pageIndex,
            int pageSize)
        {
            // الان سنقوم بمهمتين في هذه الدالة
            // الاولى سنستخدم الدالة كاونت لعد كامل سجلات الجدول بالداتا بيز 
            var count = await source.CountAsync();
            // سنستخدم دالتي السكيب والتيك لعمل اجتزاء للبيانات المطلوبة 
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            // الخطوة الاخيرة هي نعمل مثيل من الكلاس اعلاه اي استدعاء دالة البناء
            // ونمرر لها العناصر المجتزاة واجمالي سجلات الجدول بالداتا بيز اضافة لرقم الصفحة وحجم العناصر بها
            return new PaginationMaker<T>(items, count, pageIndex, pageSize);
        }

    }
}
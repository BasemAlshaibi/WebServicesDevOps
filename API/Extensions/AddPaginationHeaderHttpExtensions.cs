using System.Text.Json;
using API.Helpers;
using Microsoft.AspNetCore.Http;

namespace API.Extensions
{
     public static class AddPaginationHeaderHttpExtensions
    {
         public static void AddPaginationHeader(this HttpResponse response, int currentPage, 
            int itemsPerPage, int totalItems, int totalPages)
        {
            // هنا علشان نعمل مثيل او ابجكت من العناصر المراد ارجاعها نعمل مثيل
            // من كلاس الموديل البنجنيشن هيدر ونمرر له الاربع القيم اللي استقبلتها دال الاستاتك اعلاه
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            
            // التالي خاص بالاعدادات بحيث نجعل ملف الجيسون المرتجع بالكمل كيس
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            // الان نضيف بهيدر الريسبونس  المثيل المعرف اعلاه واللي يحمل البيانات ونمرر له الاوبشن الخاصة بالكمل كيس
            // هنا الاوبجكت سيكون له اسم هو بجنيشن
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
            // هذه الخاصية تمكنا من عمل الحقن بالهيدر ولازم نمرر لها نفس الاسم بالسطر السابق
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
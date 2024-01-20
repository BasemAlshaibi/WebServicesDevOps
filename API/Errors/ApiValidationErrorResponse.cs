using System.Collections.Generic;

namespace API.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse() : base(400)
        {
        }

        public IEnumerable<string> Errors { get; set; }
    }
}

/*
كلاس لتحسين شكل الاخطاء المرتجعة الناتجة عن اخطاء الفالديشن
هناك اعدادات موجودة بكلاس 
ApplicationServiceExtensions
خاصة بهذا ايضا
*/
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // الدالة هنا ستستقبل الريكوست واعطيناه هنا اسم كونتكست

            try
            {
// هنا مافيش مشاكل ويستمر الريكوست في رحلته أي يتم تسليمه للمادلوير التالي

                await _next(context);
            }
            catch (Exception ex)
            {
                // هنا يطبع التفاصيل حق بيانات الخطا في اللوج ايروو
                _logger.LogError(ex, ex.Message);

                //نبدا باعداد الريسبونس في حالة وجود مشكلة في السيرفر لاننا في جزء الكاتش 

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // هنا نعمل تحقق هل نحن في وقت التطوير او البرودكشن بحيث نرجع ما يناسب ذلك
                // فاذا كنا في الديفلومنت نعمل مثيل من الكلاس موديل الخاص بالاخطاء ونبعث له قيم للثلاث خصائص فيه
                // مالم نبعث كود الحالة ونصل كمسيج 
                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode, "Internal Server Error");

                // هنا نضبط تنسيق خصائص ملف الجيسون بحيث نخليها كامل كيس
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                // الان نكون ملف الجيسون وناخذ الريسبونس له مع التنسسق من المتغير اوبشن
                var json = JsonSerializer.Serialize(response, options);
                // الان نبعث بيانات الجيسون بالريسبونس
                await context.Response.WriteAsync(json);
            }
        }
    }
}
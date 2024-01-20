using System.Linq;
using API.Data;
using API.Errors;
using API.Helpers;

using AutoMapper;
 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

           

            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            // التالي من اجل نقدر نحقن بيانات الكونفجريشن والوصول للرابط الرئيسي في ملف البروفايل الخاص بالاتومابر

            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles(provider.GetService<IConfiguration>()));
            }).CreateMapper());


            services.AddDbContext<DataContext>(options =>
          {
              options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
          });


            /*
            الكود التالي متعلق بعملية تحسين اكواد الفالديشن المرتجعة للكلاينت
            بحيث انه نقدر نرتبها مثل بقية انواع الاخطاء اللي نرجعها للكلاينت
            هذا مرتبط بالكلاس
            ApiValidationErrorResponse
            اللي يورث من
            ApiResponse
            وبايكون المرتجع على الشكل
            {
    "errors": [
        "The Password field is required.",
        "The Username field is required."
    ],
    "statusCode": 400,
    "message": "A bad request, you have made"
}
            */


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });
                        

            return services;
        }
    }
}
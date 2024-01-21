using System.Linq;
using API.Data;
using API.Errors;
using API.Helpers;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using API.Data.Repositories;


namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddScoped<ITokenService, TokenService>();
            
            services.AddScoped<LogUserActivity>();

            services.AddScoped<NoOfReadIncrement>();

           services.AddScoped<IUnitOfWork, UnitOfWork>(); 
            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            // التالي من اجل نقدر نحقن بيانات الكونفجريشن والوصول للرابط الرئيسي في ملف البروفايل الخاص بالاتومابر

            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles(provider.GetService<IConfiguration>()));
            }).CreateMapper());

            

            // حزمة الكلاودينيري
            services.AddScoped<IPhotoCloudinaryService, PhotoCloudinaryService>();

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));



   
            services.AddTransient<IImageUploaderService, ImageUploaderService>();


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
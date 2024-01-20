using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using API.Data;
using API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace API
{
    public class Startup
    {
         

        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        
        public void ConfigureServices(IServiceCollection services)
        {
           //  services.AddScoped<ITokenService, TokenService>();


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
            services.AddApplicationServices(_config);
            services.AddIdentityServices(_config);
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
               // app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }
            else{
                 app.UseHsts();
                 app.UseExceptionHandler("/Home/Error"); // مؤقت
                 }

        //    app.UseMiddleware<ExceptionMiddleware>();

            /*
            السطر التالي هو في حالة انه حاول يوصل عبر اي ايند بوينت غير صحيحة مثل
            https://localhost:5001/anything
            فهنا يحوله الى المسار المحدد
            والذي ضبطناه بالكنترولر 
            ErrorController
                       */

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();
            // من أجل ملف الصور بمجلد الكونتينت
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Content")
                ), RequestPath = "/content"
            });

// must be after UseRouting and before UseEndpoints

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().AllowAnyOrigin().WithOrigins("https://localhost:4200"));

            app.UseAuthorization();

             app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}

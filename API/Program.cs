using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        // بعدنا الفويد المرتجع وخلينا اسينك تاسك
       public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build(); // حذفنا الرن وخليناه بالاخر
            // الان هنا نرريد ان نصل الى السيرفسس الخاصة بهذا المشروع
            // لانه نريد الداتا كوونتكست الخاصة بقاعدة بياناتنا
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try 
            { // نجلب الداتا كونتكست المطلوبة ونحزنها بمتغير كونتكست
                var context = services.GetRequiredService<DataContext>();

                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

                /**
                الكود التالي مهم وله فائديتن
                الاول انه لن نعود محتاجين لتنفيذ الكود الخاص بنقل عمليات الترحيل الى الداتا بيز اي هذا الامر
                dotnet ef  database update
                ووانما بمجرد تشغيل التطبيق بايعمل ابديت لكل المجريشن الموجودة ولسا ما ترحلت
                والنقطة الثانية انه لولم يكن لدينا داتا بيز موجودة لسا اي لم ننشئها
                فهنا وبطبيعة الحال ولانه سيعمل ابديت للمجريشن فيسينشئها
                ولذلك هنا سنقوم بعمل دروب للداتا بيز الموجودة لدينا
                وبمجرد نشغل البرنامج سيقوم بانشئاها وعمل سيد للبيانات العشرة فيها
                
                */
                await context.Database.MigrateAsync();               
 
                await AppIdentityDbContextSeed.SeedUsersAndRolesAsync(userManager, roleManager);
                 await StoreContextSeed.SeedAsync(context, loggerFactory);

            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during migration");
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
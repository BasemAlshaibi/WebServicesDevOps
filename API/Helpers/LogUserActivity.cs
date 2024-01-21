using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        // هذا الكلاس يحوي على دالة تستقبل اثنين برميتر
        // الاول كونتكست في حالة انه نريد انه ننفذ اللي نريده اثناء تنفيذ الريكوست
        // والبرميتر الثاني نكست وهو ينفذ الريكوست بعدين يسمح لنا انه ننفذ مانريد وهو سنستخدمه
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            // اولا سنقوم بالتاكد ان المستخدم الحالي اللي بعث الطلب هو شخص مصرح له على النظام
            // في حالة انه مستخدم مجهول فلا يعمل شيء ببساطة
            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;
            // الان نريد ان نحصل على المعرف الرقمي للمستخدم الحالي للنظام وذلك بالدالة جيت يوزر آي دي التي عملناها
            // في مجلد الاكستنش في  ملف
            //ClaimsPrincipleExtensions.cs
            var userId = resultContext.HttpContext.User.GetUserId();
            // سنحتاج كذلك لجلب السيرفس الخاصة بريبو المستخدم علشان نحصل على دالة جلب المستخدم بواسطة معرفه الرقمي
            var uow = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
            // هنا عبر الريبو نوصل لدالة جلب المستخدم بواسطة معرفه الرقمي اللي حصلنا عليه من سابق
             var user = await uow.Users.GetByIdAsync( userId); 

            // المرتجع نخزنه بمتغير باسم يوزر
            // واللي نعمله فقط انه نغير قيمته الى التاريخ الحالي
            user.LastActive = DateTime.UtcNow;
            // ثم نحفظ كل التغييرات على الداتا بيز
            await uow.Complete();
        }
    }
}

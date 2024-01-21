using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Errors;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class NoOfReadIncrement : IAsyncActionFilter
    {
        // هذا الكلاس يحوي على دالة تستقبل اثنين برميتر
        // الاول كونتكست في حالة انه نريد انه ننفذ اللي نريده اثناء تنفيذ الريكوست
        // والبرميتر الثاني نكست وهو ينفذ الريكوست بعدين يسمح لنا انه ننفذ مانريد وهو  ما سنستخدمه
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            string path = resultContext.HttpContext.Request.Path;
 
                var newsId = Convert.ToInt32(path.Substring(path.LastIndexOf('/') + 1));

                var uow = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();

                News news = await uow.NewsDashboard.GetByIdAsync(newsId);

                if (news is not null){ // فقط يعدل لو في خبر موجود بنفس الايدي
                 news.NoOfRead = news.NoOfRead + 1;
                }
 
                 await uow.Complete();
            
 
        }



    }
}
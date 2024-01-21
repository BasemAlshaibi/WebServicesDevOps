using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    /*
       هذا الكلاس يعالج مسالة محاولة طلب اي اند بوينت غير موجودة
       اي  انه حاول يوصل عبر اي ايند بوينت غير صحيحة مثل
           https://localhost:5001/anything
           فهنا لدينا سطر في كلاس الاستارت اب هو
         app.UseStatusCodePagesWithReExecute("/errors/{0}");
           و يحوله الى الكلاس
           بحيث ننشى اوبجكت من المرتجع من كلاس
           ApiResponse
           اللي نضبط به شكل الخطا المرتجع عموما
            
    
    */
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}
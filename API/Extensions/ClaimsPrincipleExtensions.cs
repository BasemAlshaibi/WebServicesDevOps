using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
     

           public static string GetUsername(this ClaimsPrincipal user)
        {
         //نستقبل البرميتر اي الكلاس اللي سنستدعي فيها دالة جلب اسم المستخدم ونرجع اسم المستخدم الحالي 

            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        { // هنا سنسترجع المعرف الرقمي للمستخدم الحالي لذا عملنا كاستنج
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }


    }
}
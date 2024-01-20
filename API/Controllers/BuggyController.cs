using System;
using API.Data;
using API.Entities;
using API.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        // للتطبيق فقط على الاخطاء

                            /*
            هنا عندما يطلب هذه الاند بوينت سنرد له بباد ريكوست
            والكود هنا
            400
            */

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
            //   return BadRequest();

        }

        /*
        الدالة الاولى اذا طلبناها ونحن ما عملنا لوجن
        اي ما ارسلنا توكن بهيدر الركوست فسترد علينا 401
        اي
        Unauthorized
        */
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }
        

        /*
        هنا  سنطلب معلومات عن شخص مش متوفر وبالتالي نرجع الخطا
        404
        Not Found
        */
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);

            if (thing == null) return NotFound(new ApiResponse(404));

            return Ok(thing);
        }
        /*
هنا سنصنع خطا برمجي بانه نطبق دالة توسترنج الى متغير قيمته نل
بالتالي الخطا هنا خطا سيرفر اي 501
*/

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context.Users.Find(-1);

            var thingToReturn = thing.ToString();

            return thingToReturn;
        }

    }
}
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        // عملنا حقن في دالة البناء للكونفجريشن علشان نوصل لمفتاح التوكن المعرف في ملف
        // appsettings.Development.json 
        // ثم عملنا له تشغير متماثل بعد جلبه  واسنداه لمتغير باسم كي 
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;
        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        //دالة انشاء التوكن ستستقبل سجل من بيانات المستخدم وهذا علشان ناخذ منه معلومة الاسم ونضيفه ككليم
        public async Task<string> CreateToken(AppUser user)
        {
            // الخطوات هنا لا تختلف عن ما تعلمناه في دروس الادنتتي فنحضر الكليمز والتوقيع
            var claims = new List<Claim>
            {
                // هنا سنضيف اثنين كليمز واحد للمعرف الرقمي للمستخدم والاخر لاسمه 
                // هذه القيمتين مهمة بحيث انه بالباك اند نعرف المستخدم الحالي كم رقمه او اسمه
                // بدون الحاجة الى انه الكلاينت يرسل لنا ذلك
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),    
         };

// نجلب كل الرولز الخاصة بالمستخدم 
          var roles = await _userManager.GetRolesAsync(user);
// الان نضيفها الى مصفوفة الكليمز بجوار العنصرين اللي بالاعلى
// وهنا عملنا اسم نوع الكليم تايب مخصص وسميناها رول وقيمتها ستكون الرول اللي واجيه من الداتا بيز
// والكود التالي هو كود مختصر لما تعلمناه في عمل لووب من مصفوفة لاخرى
         claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

// التوقيع هنا يستقبل المفتاح المشفر واسم خوارزميرة التشفير
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
// هنا نحضر معلومات التوكن من كليمز ووقت انتهاء والتوقيع المشفر
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // ينتهي بعد سبعة ايام من تاريخ انشاءه
                SigningCredentials = creds
            };

//بقية الخطوات متعلقة بانشاء هندلر نمرر له معلومات التوكن ثم انشاءه وارجاعه لمن يطلب هذه الدالة
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

    }

}
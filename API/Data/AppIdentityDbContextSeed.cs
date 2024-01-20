using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace API.Data
{
    public class AppIdentityDbContextSeed
    {
        // دالة ستاتك سنستدعيها كملحقة
        public static async Task SeedUsersAndRolesAsync(UserManager<AppUser> userManager,
         RoleManager<AppRole> roleManager)
        {

        

            // نتحقق في حالة وجود بيانات في جدول المستخدمين فلا حاجة لاستكمال الكود وزرع بيانات
            if (await userManager.Users.AnyAsync()) return;

               var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // نجلب البيانات من ملف الجيسون ونعمل لها دي ريلايز لنقلها الى مثيل من كلاس المستخدم
            var userData =  File.ReadAllText(path + @"/Data/SeedData/UserSeedData.json");

           
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (users == null) return;

            // ننشى لسته فيها الرولز المطلوب اضافتها ثم نعمل لها انشاء في الداتا بيز واحدة تلو اخرى
            var roles = new List<AppRole>
            {
                new AppRole{Name = "Editor"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Author"},
                new AppRole{Name = "Contributor"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            // الان نضيف قائمة المستخدمين اللي جلبناها من ملف الجيسون واحد تو اخر 
            // نحرص على انه نجعل اسماء المستخدمين بحروف صغيرة ثم ننشى مع تمرير باسورد لهم
            // وبالاخير وافتراضيا نعطي المستخدمين العشرة جميعا الرولز ممبر اي اعضاء 

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Author");
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };

            // نريد كمان يعمل لنا زرع بانشاء مستخدم خصوصي باسم ادمن يكون له رولز ادمن ومشرف مع بعض
            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin"});
        }
    }
}
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
 

 // for Dashboard
    IGenericRepository<AppUser,UserResponseDto> Users {get;}
     IGenericRepository<News,NewsRespDashboardDto> NewsDashboard {get;}

     IGenericRepository<Category,CategoryDashboardDto> CategoriesDashboard {get;}

     

 
    // خاص بجلب الاقسام وعدد من الاخبار ضمن كل قسم للواجهة الرئيسية
    IGenericRepository<Category,CategoryVistorsDto> CategoriesVistors {get;}



    // خاص بجلب الاخبار للواجهة الرئيسية
    IGenericRepository<News,NewsRespHomeVistorsDto> NewsHomeVistors {get;}

    IGenericRepository<News,NewsRespDetailsVistorsDto> NewsDetailsVistors {get;}

  
 

// حقن لصور الاخبار  حاول تغير اسم كلاس الديتو الراجع او بقيه لانه سيكون مشترك مع الكتاب
    IGenericRepository<NewsImage,ImageRespInfoDto> NewsImage  {get;}

 

        Task<bool> Complete(); // الدالة اللي سننفذ بها السيف شنج باخر كل عملية كروود
        bool HasChanges(); //  في حالة وجود تعديل تعيد بترو
    }
}
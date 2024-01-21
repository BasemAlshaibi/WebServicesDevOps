using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.Processing;

namespace API.Services
{
    public class PhotoCloudinaryService : IPhotoCloudinaryService
    {
        // نعمل مثيل من كلاس الكلاونيري
        private readonly Cloudinary _cloudinary;

        // التالي كود لجلب البيانات التي في الكونفجريشن عبر الكلاس الوسيط
        public PhotoCloudinaryService(IOptions<CloudinarySettings> config)
        {
            // نمرر القيم الثلاث بالترتيب الى مثيل من كلاس الاكاونت
            var acc = new Account
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            // الان نمرر المثيل من كلاس الاكاونت الى مثيل من كلاس الكلاودينري ونخزنه فيه
            _cloudinary = new Cloudinary(acc);
        }

        // هذه دالة رفع الصورة الى السحابة وتستقبل ملف 
        public async Task<ImageUploadResult> AddPhotoToCloudinaryAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult(); //  مثيل سنتعامل معه لاحقا

            if (file.Length > 0) // نتحقق ان الملف موجود عبر مساحته
            {
                using var stream = file.OpenReadStream(); // نفتح الاستريم من الملف
                var uploadParams = new ImageUploadParams // نجهز البرميترز للرفع
                {
                    File = new FileDescription(file.FileName, stream), // هنا سيحتاج مننا اسم الملف والاسترين
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                    // الدالة السابقة تجري تحولات قبل الرفع بحيث نضبط الطول والعرض بخمسمية بكسل
                    // وكذلك يعمل ازالة للمتبقي ويركز على الوجه اي يخليه بالسنتر وهذا يعمله تلقائي
                };

                // يستدع دالة الرفع ويمرر لها البرميترات والمرتجع يرجعه لنا
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        // دالة الحذف تستقبل ببلك آي دي
        public async Task<DeletionResult> DeletePhotoFromCloudinaryAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }

 


      
    }
}
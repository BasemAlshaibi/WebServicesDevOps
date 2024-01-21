using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IPhotoCloudinaryService
    {
        Task<ImageUploadResult> AddPhotoToCloudinaryAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoFromCloudinaryAsync(string publicId);
 

    }
    /*
    الانواع المرتجعة في الدالتين توفرها لنا حزمة الكلاودنيري
    
    */

}
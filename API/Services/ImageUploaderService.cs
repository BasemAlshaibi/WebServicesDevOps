using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace API.Services
{
    public class AddedImageInfo
    {
        public Guid Id { get; set; }

        public string Folder { get; set; }
    }

    public class ImageUploaderService : IImageUploaderService

    {

        private const int ThumbnailWidth = 200;
        private const int FullscreenWidth = 1000;


        public async Task<List<AddedImageInfo>> Process(IEnumerable<ImageInputDto> images)
        {
            var addedImageInfo = new List<AddedImageInfo>();

            var tasks = images
            .Select(image => Task.Run(async () =>
            {
                using var imageResult = await Image.LoadAsync(image.Content); // استخدمنا مكتبة سيكس لابور

                // الان نجهز لمسار الصورة واسمها
                DateTime dt = DateTime.Now;
                string year = dt.Year.ToString();
                string month = dt.Month.ToString();
                string day = dt.Day.ToString();

                var id = Guid.NewGuid();
                var path = $"/images/{year}/{month}/{day}/";
                var name = $"{id}.jpg";


              //  var storagePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{path}".Replace("/", "\\"));
                var storagePath = Path.Combine(Directory.GetCurrentDirectory(), $"Content{path}".Replace("/", "\\"));

                if (!Directory.Exists(storagePath))
                {
                    Directory.CreateDirectory(storagePath);
                }

                await SaveImage(imageResult, $"Original_{name}", storagePath, imageResult.Width);
                await SaveImage(imageResult, $"Fullscreen_{name}", storagePath, FullscreenWidth);
                await SaveImage(imageResult, $"Thumbnail_{name}", storagePath, ThumbnailWidth);


                addedImageInfo.Add(new AddedImageInfo()
                {
                    Id = id,
                    Folder = path,
                });


            })).ToList();

            await Task.WhenAll(tasks);

            return addedImageInfo;
        }


        private async Task SaveImage(Image image, string name, string path, int resizeWidth)
        {
            var width = image.Width;
            var height = image.Height;

            if (width > resizeWidth)
            {
                height = (int)((double)resizeWidth / width * height);
                width = resizeWidth;
            }

            image.Mutate(i => i.Resize(new Size(width, height)));

            image.Metadata.ExifProfile = null;

            await image.SaveAsJpegAsync($"{path}/{name}", new JpegEncoder
            {
                Quality = 75
            });

        }


    }
}
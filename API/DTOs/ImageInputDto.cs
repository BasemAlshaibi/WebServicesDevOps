using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ImageInputDto
    {

        // كلاس للاستخدام اثناء تمرير ملف الصورة لعمل معالجة له
        public string Name { get; set; }
        public string Type { get; set; }

        public Stream Content { get; set; }
 
    }
}
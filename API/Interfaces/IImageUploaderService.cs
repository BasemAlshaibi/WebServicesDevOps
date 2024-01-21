using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Services;

namespace API.Interfaces
{
    public interface IImageUploaderService 
     {
             public Task<List<AddedImageInfo>> Process(IEnumerable<ImageInputDto> images); 

    }
}
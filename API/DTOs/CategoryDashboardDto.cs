using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class CategoryDashboardDto
    {
         public int? Id { get; set; } // اوبشنل علشان وقت الاضافة بالابديت الجماعي ما يسبب مشكلة

         public string Name { get; set; } 

         public bool Status { get; set; }  

         public int Order { get; set; }         
               

     //   public ICollection<NewsResponseDto> News { get; set; }

         public string NameCategoryUrl { get; set; }
 
    }
}
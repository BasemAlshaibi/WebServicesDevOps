using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{ 
    public class Category
    {   //  [Key]
          public int Id { get; set; }

          public string Name { get; set; } 

          public bool Status { get; set; }  = true;  // غيره لاحقا الى اس اكتف

          public ICollection<News> News { get; set; }

     //     [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          public int Order { get; set; }


      //     public string NameCategoryUrl { get; set; }

 

    }
  
  
}
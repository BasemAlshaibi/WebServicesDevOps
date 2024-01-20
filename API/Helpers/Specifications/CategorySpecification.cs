using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Helpers.Parameters;

namespace API.Helpers.Specifications
{
    public class CategorySpecification :BaseSpecifcation<Category>
    {


 

         public CategorySpecification(CategoryParams categoryParams)  : base(x =>  ( categoryParams.isActive.HasValue ? x.Status == categoryParams.isActive :( x.Status || !x.Status )  ))
        {
           AddOrderBy(n => n.Order);
         //  ApplyPaging(categoryParams.PageIndex , categoryParams.PageSize);
        }
         public CategorySpecification(int id) : base(x => x.Id == id)
        {
        }
         
        public CategorySpecification(string name) : base(x => x.Name.ToLower().Replace(" ", "_") == name.ToLower().Replace(" ", "_"))
        { 
        }
    }
}
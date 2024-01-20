using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Helpers.Specifications
{
    public class UserSpecification: BaseSpecifcation<AppUser>
    {
        public UserSpecification(UserSpecParams  userSpecParams ) : base(x => 
            (string.IsNullOrEmpty(userSpecParams.Search) || x.KnownAs.ToLower().Contains(userSpecParams.Search)) 
        )
        {

         //  AddOrderByDescending(x => x.LastActive);   
         //  AddOrderBy(x => x.Created);       

           ApplyPaging(userSpecParams.PageIndex , userSpecParams.PageSize);

            if (!string.IsNullOrEmpty(userSpecParams.Sort))
            {
                switch (userSpecParams.Sort)
                {
                    case "created":
                        AddOrderBy(n => n.Created);
                        break;
                    case "topActive":
                        AddOrderByDescending(u => u.CreatedUsers.Count);
                        break;
                     case "name":
                        AddOrderBy(u => u.UserName);
                        break;
                    default:
                        AddOrderByDescending(u => u.LastActive);
                        break;
               
                }
            }

            

        }

         public UserSpecification(int id) : base(x => x.Id == id)
        {
           AddInclude(x => x.UserPhotos);
        }

        public UserSpecification(string name) : base(x => (string.IsNullOrEmpty(name) ||  x.UserName.ToLower() == name.ToLower()))
        { 
        }
    }
}
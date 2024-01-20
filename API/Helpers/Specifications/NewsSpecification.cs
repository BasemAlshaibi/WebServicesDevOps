using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Helpers.Specifications
{
    public class NewsSpecification : BaseSpecifcation<News>
    {
        public NewsSpecification(NewsSpecParams newsSpecParams) : base(x => 
            (string.IsNullOrEmpty(newsSpecParams.Search) || x.Title.ToLower().Contains(newsSpecParams.Search)) &&
            ( string.IsNullOrEmpty(newsSpecParams.Category) || x.Category.Name.ToLower()== newsSpecParams.Category.ToLower()) &&
             (!newsSpecParams.CategoryId.HasValue || x.CategoryId == newsSpecParams.CategoryId) &&
            (string.IsNullOrEmpty(newsSpecParams.CreatedBy) || x.CreatedUser.UserName.ToLower() == newsSpecParams.CreatedBy.ToLower())&&
            ( x.CreatedAt >= newsSpecParams.MinDate && x.CreatedAt <= newsSpecParams.MaxDate)&&
            (  string.IsNullOrEmpty(newsSpecParams.Status) ||  x.Status ==  Enum.Parse<PublishingStatus>(newsSpecParams.Status))
            && ( newsSpecParams.isShowInMain.HasValue ? x.isShowInMain == newsSpecParams.isShowInMain :( x.isShowInMain || !x.isShowInMain )  )
              && ( newsSpecParams.isChooseEditor.HasValue ? x.isChooseEditor == newsSpecParams.isChooseEditor :( x.isChooseEditor || !x.isChooseEditor )  )
                && ( newsSpecParams.isBreakingOrImportant.HasValue ? x.IsBreakingOrImportant == newsSpecParams.isBreakingOrImportant :( x.IsBreakingOrImportant || !x.IsBreakingOrImportant )  )     
        )
        { 


          AddOrderByDescending(x => x.PublishedAt);     

          ApplyPaging(newsSpecParams.PageIndex , newsSpecParams.PageSize);

          ApplyTop(newsSpecParams.Top); // تحت التجربة


            if (!string.IsNullOrEmpty(newsSpecParams.Sort))
            {
                switch (newsSpecParams.Sort)
                {
                    case "TopNews":
                        AddOrderByDescending(n => n.NoOfRead);
                        break;
                    default:
                        AddOrderByDescending(n => n.PublishedAt);
                        break;
               
                }
            }

            /*
           // because we use  Direct mappint  i.e ProjectTo<TResponse>(_mapper.ConfigurationProvider) , so we dont need send entity to Includes

              AddInclude(x => x.Category);
              AddInclude(x => x.CreatedUser);
               AddInclude(x => x.UpdatedUser);
               AddInclude(x => x.ImageFiles);
            
            */


           /* 
             because we use  PaginationMaker to Apply Paging, so we dont need ApplyPaging here  

           ApplyPaging(newsSpecParams.PageSize * (newsSpecParams.PageNumber - 1), newsSpecParams.PageSize);
*/

        }

         public NewsSpecification(int id) : base(x => x.Id == id)
        {
              AddInclude(x => x.NewsImages); 
              AddInclude(x => x.CreatedUser); 

              AddInclude(x => x.UpdatedUser); 
        }
    }
}
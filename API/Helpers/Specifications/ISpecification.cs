using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace API.Helpers.Specifications
{

    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }


        int PageIndex { get; }
        int PageSize { get; }

        int Top { get; }


        /*
        because we use  PaginationMaker to Apply Paging, so we dont need Take and Skip here , only PageNumber ,PageSize
        */
        // bool IsPagingEnabled { get; }

        // int Take { get; }
        // int Skip { get; }

        /*
        because we use  Direct mapping  i.e ProjectTo<TResponse>(_mapper.ConfigurationProvider) , so we dont need send entity to Includes
        */
         List<Expression<Func<T, object>>> Includes { get; }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Helpers.Specifications
{
    public class BaseSpecifcation<T> : ISpecification<T>
    {
        public BaseSpecifcation()
        {
        }

        public BaseSpecifcation(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }


        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

         public int Top { get; private set; }



        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        protected void ApplyPaging(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

         protected void ApplyTop(int top)
        {
            Top = top;
        }


  


        /* 
       // because we use  Direct mappint  i.e ProjectTo<TResponse>(_mapper.ConfigurationProvider) , so we dont need send entity to Includes
       // لكن سنفعلها من اجل  الصور    لجلبها  وارجاع الداتا الاصليبة بدون مابنج وكذلك للاخبار داخل الكاتيجري
         */

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }


        /*

       //because we use  PaginationMaker to Apply Paging, so we dont need Take and Skip here , only PageNumber ,PageSize

         public int Take { get; private set; }

        public int Skip { get; private set; }
         public bool IsPagingEnabled { get; private set; }

         protected void ApplyPaging(int skip, int take)
          {
              Skip = skip;
              Take = take;
              IsPagingEnabled = true;
          }

        */


    }

}
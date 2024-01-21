using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Helpers;
using API.Helpers.Specifications;

namespace API.Interfaces
{
   public interface IGenericRepository<TRequest,TResponse> 
    where TRequest : class
    where TResponse : class
    {
     
     
 
       // Get List
        Task<PaginationMaker<TResponse>> GetAllWithSpec(ISpecification<TRequest> spec); 

        Task<IReadOnlyList<TResponse>> GetAllWithoutSpec();

         Task<IReadOnlyList<TResponse>> GetAllWithSpecWithoutPaging(ISpecification<TRequest> spec); 


        // Get One Entit 


        Task<TRequest> GetByIdAsync(int id);
 
        Task<TResponse> GetEntityWithSpec(ISpecification<TRequest> spec);

        Task<TRequest> GetEntityWithSpecWithOutMap(ISpecification<TRequest> spec);

 

 


//Editing operations
        Task  AddAsync(TRequest entity); // عالج من اجل امكانية ارجاع العنصر اللي انضف وكذلك اللي بعدها
        Task  AddRangeAsync(IEnumerable<TRequest> entities);



        void Update(TRequest entity);
        void UpdateRange(IEnumerable<TRequest> entities); 
        void Delete(TRequest entity);




 
 
    }
}
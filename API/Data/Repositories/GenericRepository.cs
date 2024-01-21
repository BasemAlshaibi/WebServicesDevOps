using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Helpers;
using API.Helpers.Specifications;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class GenericRepository<TRequest, TResponse> : IGenericRepository<TRequest, TResponse>
    where TRequest : class
    where TResponse : class
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public GenericRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        // Get One Entity



        public async Task<TResponse> GetEntityWithSpec(ISpecification<TRequest> spec)
        {
            return await ApplySpecification(spec).ProjectTo<TResponse>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<TRequest> GetEntityWithSpecWithOutMap(ISpecification<TRequest> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }


        public async Task<TRequest> GetByIdAsync(int id)
        {
            return await _context.Set<TRequest>().FindAsync(id);
        }



        // Get All

        public async Task<PaginationMaker<TResponse>> GetAllWithSpec(ISpecification<TRequest> spec)
        {

            var query = ApplySpecification(spec);
            return await PaginationMaker<TResponse>.CreateAsync(query.ProjectTo<TResponse>(_mapper.ConfigurationProvider).AsNoTracking(), spec.PageIndex, spec.PageSize);

        }

        public async Task<IReadOnlyList<TResponse>> GetAllWithoutSpec()
        {
            return await _context.Set<TRequest>()
            .ProjectTo<TResponse>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

         public async Task<IReadOnlyList<TResponse>> GetAllWithSpecWithoutPaging(ISpecification<TRequest> spec)
        {
            var query = ApplySpecification(spec);
           return  await query.ProjectTo<TResponse>(_mapper.ConfigurationProvider).AsNoTracking().ToListAsync();
 
        }

        private IQueryable<TRequest> ApplySpecification(ISpecification<TRequest> spec)
        {
            return SpecificationEvaluator<TRequest>.GetQuery(_context.Set<TRequest>().AsQueryable(), spec);
        }


        //Add operations
        public async Task AddAsync(TRequest entity)
        {
            await _context.Set<TRequest>().AddAsync(entity);
            // _context.SaveChanges(); // تحت التجربة
            //return entity;
        }

        public async Task AddRangeAsync(IEnumerable<TRequest> entities)
        {
             await _context.Set<TRequest>().AddRangeAsync(entities);
           //  _context.SaveChanges(); // تحت التجربة
          //  return entities;
        }


        //Editing operations



        public void Update(TRequest entity)
        {
           //  _context.Set<TRequest>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

        }

        public void UpdateRange(IEnumerable<TRequest> entities)
        {
            //  _context.Set<TRequest>().AttachRange(entities);

            _context.Set<TRequest>().UpdateRange(entities);
            //  _context.UpdateRange(entities);
        }

        public void Delete(TRequest entity)
        {
            _context.Set<TRequest>().Remove(entity);
        }

   
    }
}
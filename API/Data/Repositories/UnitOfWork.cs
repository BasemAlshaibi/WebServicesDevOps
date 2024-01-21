using System.Threading.Tasks;
using API.Data.Repositories;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    { // الخطوة الاولى نعمل حقن للسيرفس اللي باتحتاجها لاحقا هذه الريبستوريس
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        // generic -- Dashboard


        public IGenericRepository<AppUser, UserResponseDto> Users => new GenericRepository<AppUser, UserResponseDto>(_context, _mapper);


        


        public IGenericRepository<Category, CategoryDashboardDto> CategoriesDashboard => new GenericRepository<Category, CategoryDashboardDto>(_context, _mapper);

        public IGenericRepository<News, NewsRespDashboardDto> NewsDashboard => new GenericRepository<News, NewsRespDashboardDto>(_context, _mapper);


        // generic -- Vistors Pages

        public IGenericRepository<Category, CategoryVistorsDto> CategoriesVistors => new GenericRepository<Category, CategoryVistorsDto>(_context, _mapper);


        public IGenericRepository<News, NewsRespHomeVistorsDto> NewsHomeVistors => new GenericRepository<News, NewsRespHomeVistorsDto>(_context, _mapper);


        public IGenericRepository<News, NewsRespDetailsVistorsDto> NewsDetailsVistors => new GenericRepository<News, NewsRespDetailsVistorsDto>(_context, _mapper);


        public IGenericRepository<NewsImage, ImageRespInfoDto> NewsImage => new GenericRepository<NewsImage, ImageRespInfoDto>(_context, _mapper);

  

        // ثم ننفذ الدالتين المعرفتين بالانترفيس
        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
            //true if there are changes to save, otherwise false.

        }
    }
}
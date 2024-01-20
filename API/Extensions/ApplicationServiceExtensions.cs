using System.Linq;
using API.Data;
using AutoMapper;
 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
          {
              options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
          });

                        

            return services;
        }
    }
}
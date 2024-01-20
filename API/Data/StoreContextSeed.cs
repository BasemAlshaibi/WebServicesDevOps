using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.Extensions.Logging;

namespace API.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(DataContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (!context.Categories.Any())
                {
                    var CategoryData = File.ReadAllText(path + @"/Data/SeedData/Category.json");
                    var categories = JsonSerializer.Deserialize<List<Category>>(CategoryData);

                    foreach (var item in categories)
                    {
                        context.Categories.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.News.Any())
                {
                    var NewsData = File.ReadAllText(path + @"/Data/SeedData/News.json");
                    var news = JsonSerializer.Deserialize<List<News>>(NewsData);

                    foreach (var item in news)
                    {
                        context.News.Add(item);
                    }

                    await context.SaveChangesAsync();
                }
    

 

              
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
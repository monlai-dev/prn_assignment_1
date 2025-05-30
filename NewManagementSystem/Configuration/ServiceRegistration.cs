using Microsoft.EntityFrameworkCore;
using NewManagementSystem.Services.Abstractions;
using NewManagementSystem.Repository;
using NewManagementSystem.Repository.Abstractions;
using NewManagementSystem.Services;
using NewsManagementSystem.DataAccess;
using NewsManagementSystem.DataAccess.Repository.Abstractions;
using NewsManagementSystem.Services.Services;

namespace NewsManagementSystem.WebMVC.Configuration
{
    public static class ServiceRegistration
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            var services = builder.Services;

            // Register EF Core DbContext
            services.AddDbContext<FunewsManagementContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register Repositories
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IArticleRepository, ArticlesRepository>(); 

            // Register Services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IArticleService, ArticleService>(); 
        }
    }
}
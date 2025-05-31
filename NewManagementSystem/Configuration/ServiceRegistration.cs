using Microsoft.EntityFrameworkCore;
using NewManagementSystem.Services.Abstractions;
using NewManagementSystem.Repository;
using NewManagementSystem.Repository.Abstractions;
using NewManagementSystem.Services;
using NewsManagementSystem.DataAccess;

namespace NewManagementSystem.Configuration
{
    public static class ServiceRegistration
    {
        // Phương thức mở rộng để đăng ký các service
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            // Đăng ký các repository
            builder.Services.AddDbContext<FunewsManagementContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
            
            // Đăng ký các Service
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
        }
    }
}

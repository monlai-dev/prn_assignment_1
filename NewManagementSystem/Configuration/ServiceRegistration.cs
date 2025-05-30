using Microsoft.EntityFrameworkCore;
using NewManagementSystem.Services.Abstractions;
using NewManagementSystem.Repository;
using NewManagementSystem.Repository.Abstractions;
using NewManagementSystem.Services;
using NewsManagementSystem.DataAccess;
using NewsManagementSystem.Services.Services.Abstractions;
using NewsManagementSystem.Services.Services;
using NewsManagementSystem.DataAccess.Repository.Abstractions;
using NewsManagementSystem.DataAccess.Repository;

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

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            // Đăng ký các Service
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddScoped<ICategoryServices, CategoryServices>();

        }
    }
}

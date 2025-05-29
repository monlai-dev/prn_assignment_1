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
        // Phương thức mở rộng để đăng ký các service
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            // Đăng ký các repository
            builder.Services.AddDbContext<FunewsManagementContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            
            // Đăng ký các Service
            builder.Services.AddScoped<IAccountService, AccountService>();
        }
    }
}

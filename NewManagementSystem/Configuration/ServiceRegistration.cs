using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NewManagementSystem.Repository;
using NewManagementSystem.Repository.Abstractions;
using NewManagementSystem.Services;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.DataAccess;
using NewsManagementSystem.DataAccess.Repository;
using NewsManagementSystem.DataAccess.Repository.Abstractions;
using NewsManagementSystem.Services.Services;
using NewsManagementSystem.Services.Services.Abstractions;

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

            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();            
			      builder.Services.AddScoped<INewsRepository, NewsRepository>();

			      // Đăng ký các Service
			      builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<INewsService, NewsService>();
            builder.Services.AddScoped<ICategoryServices, CategoryServices>();

            // Cấu hình OAuth
            builder.Services.AddAuthentication(options =>
            {
              options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
              options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
             .AddCookie(options =>
             {
               options.LoginPath = "/Login";
               options.AccessDeniedPath = "/Home";
               options.LogoutPath = "/Logout";
               options.Cookie.Name = "Authentication";
             })
             .AddGoogle(googleOptions =>
             {
               googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
               googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
             });
		    }
    }
}
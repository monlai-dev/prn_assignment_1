using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NewManagementSystem.Repository;
using NewManagementSystem.Repository.Abstractions;
using NewManagementSystem.Services;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.DataAccess;
using NewsManagementSystem.DataAccess.Repository;
using NewsManagementSystem.DataAccess.Repository.Abstractions;
using NewsManagementSystem.Services.Services;
using NewsManagementSystem.Services.Services.Abstractions;

namespace NewsManagementSystem.Web.Configuration
{
    public static class ServiceRegistration
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var adminCredentials = configuration.GetSection("AdminAccount").Get<AdminCredentialsModel>();
            services.AddSingleton(adminCredentials);

            // Register EF Core DbContext
            services.AddDbContext<FunewsManagementContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
            services.AddScoped<IArticleRepository, ArticlesRepository>();
            // Đăng ký các Service
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<INewsArticleService, NewsArticleService>();
            services.AddScoped<IArticleService, ArticleService>();


            // Cấu hình OAuth
            services.AddAuthentication(options =>
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
                 googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                 googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
             });
        }
    }
}

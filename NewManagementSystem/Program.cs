using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using NewManagementSystem.Configuration;
using NewsManagementSystem.DataAccess.Repository;
using NewsManagementSystem.DataAccess.Repository.Abstractions;
using NewsManagementSystem.Services;
using NewsManagementSystem.Services.Services;
using NewsManagementSystem.Services.Services.Abstractions;

namespace NewManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Đăng ký controller
            builder.Services.AddControllersWithViews();

            // Gọi method cấu hình services trong class ServiceRegistration
            builder.ConfigureServices();

            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
            builder.Services.AddScoped<INewsArticleService, NewsArticleService>();

            var app = builder.Build();

            // Middleware pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

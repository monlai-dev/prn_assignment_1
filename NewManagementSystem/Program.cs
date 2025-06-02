using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using NewsManagementSystem.WebMVC.Configuration;
using System.Security.Claims;

namespace NewsManagementSystem.WebMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load config from files and env vars
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            builder.Services.AddControllersWithViews();
            builder.ConfigureServices();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

			app.Use(async (context, next) =>
			{
				var user = context.User;
				var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
				var path = context.Request.Path.ToString().ToLower();
				if (user.Identity?.IsAuthenticated == true)
				{
					var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
					logger.LogInformation($"Detected Role: {role}");
					logger.LogInformation($"Current Path: {path}");
					// Nếu chưa đăng nhập, chuyển hướng về Home/Index
					if (user.Identity?.IsAuthenticated != true)
					{

						context.Response.Redirect("/Home/Index");
						return;
					}
					if (path == "/account/logout")
					{
						await next();
						return;
					}
					if (role == "3" && !path.StartsWith("/admin"))
					{
						logger.LogWarning("Redirect 3 to Admin");
						context.Response.Redirect("/Admin/Users");
						return;
					}
					if (role == "2" &&
							!(path.StartsWith("/newsarticles") || path.StartsWith("/tags") || path.StartsWith("/category")))
					{
						logger.LogWarning("Redirect 2 to NewsArticles page");
						context.Response.Redirect("/NewsArticles");
						return;
					}


				}
				await next();
			});
			app.MapControllerRoute(
			  name: "default",
			  pattern: "{controller=Home}/{action=Index}/{id?}");

			app.MapControllerRoute(
				name: "staff",
				pattern: "{controller=NewsArticles}/{action=index}/{id?}");

			app.MapControllerRoute(
				name: "admin",
				pattern: "{controller=Admin}/{action=Users}/{id?}");

            app.Run();
        }
    }
}
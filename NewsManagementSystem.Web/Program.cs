using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using NewsManagementSystem.Web.Configuration;

namespace NewsManagementSystem.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Load config từ file + môi trường
			builder.Configuration
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();

			// Thêm Razor Pages
			builder.Services.AddRazorPages();
            builder.Services.AddSignalR();

			builder.Services.ConfigureServices(builder.Configuration);

            var app = builder.Build();

			// Xử lý lỗi và HSTS
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			//// Nếu có authentication, thêm dòng này:
			//// app.UseAuthentication(); 
			app.UseAuthorization();

			// Middleware phân quyền giống MVC
			app.Use(async (context, next) =>
			{
				var user = context.User;
				var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
				var path = context.Request.Path.ToString().ToLower();

				if (user.Identity?.IsAuthenticated == true)
				{
					var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
					logger.LogInformation($"[RAZOR] Detected Role: {role}");
					logger.LogInformation($"Current Path: {path}");

					// Không cần kiểm lại IsAuthenticated ở đây vì đã check bên trên
					if (path == "/account/logout")
					{
						await next();
						return;
					}

					if (role == "1" &&
						!(path.StartsWith("/newsarticles") || path.StartsWith("/tags") || path.StartsWith("/category")))
					{
						logger.LogWarning("Redirect (Role 1) to /NewsArticles");
						context.Response.Redirect("/NewsArticles");
						return;
					}

					if (role == "3" && !path.StartsWith("/admin"))
					{
						logger.LogWarning("Redirect (Role 3) to /Admin/Users");
						context.Response.Redirect("/Admin/Users");
						return;
					}
				}

				await next();
			});

			// Map Razor Pages
			app.MapRazorPages();
            app.MapHub<DataHub>("/hub");
            app.MapHub<DataHub>("/dataHub");


            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //    endpoints.MapHub<DataHub>("/hub");
            //});

            app.Run();
		}
	}
}

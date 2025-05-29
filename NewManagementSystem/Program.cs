using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using NewsManagementSystem.WebMVC.Configuration;

namespace NewsManagementSystem.WebMVC
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

using NewsManagementSystem.BusinessObject.Models;

namespace NewsManagementSystem.Services.Services.Abstractions
{
	public interface INewsService
	{
		List<NewsArticle> GetAllNewsWithDetails();
	}
}

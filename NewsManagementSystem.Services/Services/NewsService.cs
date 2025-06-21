using NewsManagementSystem.DataAccess.Repository.Abstractions;
using NewsManagementSystem.Services.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Models;


namespace NewsManagementSystem.Services.Services
{
	public class NewsService : INewsService
	{
		private readonly INewsRepository _newsRepository;

		public NewsService(INewsRepository newsRepository)
		{
			_newsRepository = newsRepository;
		}

		public List<NewsArticle> GetAllNewsWithDetails()
		{
			return _newsRepository.GetAllNewsWithDetails();
		}
	}
}

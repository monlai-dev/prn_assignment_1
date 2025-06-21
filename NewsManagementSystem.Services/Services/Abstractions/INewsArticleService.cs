

using NewManagementSystem.Models;
using NewsManagementSystem.BusinessObject.Models;

namespace NewsManagementSystem.Services.Services.Abstractions
{
    public interface INewsArticleService
    {
        Task<List<NewsArticle>> GetAllAsync();
        Task<NewsArticle> GetByIdAsync(int id);
        Task CreateAsync(NewsArticle article);
        Task UpdateAsync(NewsArticle article);
        Task DeleteAsync(int id);

        Task<List<Tag>> GetAllTagsAsync();
        Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds);

        Task<List<Category>> GetAllCategoriesAsync();
        Task<List<SystemAccount>> GetAllSystemAccountsAsync();
        Task<SystemAccount> GetUserById(int id);

        Task<int> GetMaxNewsArticleIdAsync();
    }
}

using NewsManagementSystem.BusinessObject.Models;
using NewManagementSystem.Models;


namespace NewsManagementSystem.DataAccess.Repository.Abstractions
{
    public interface INewsArticleRepository
    {
        Task<List<NewsArticle>> GetAllAsync();
        Task<NewsArticle> GetByIdAsync(int id);
        Task CreateAsync(NewsArticle article);
        Task UpdateAsync(NewsArticle article);
        Task SoftDeleteAsync(int id);

        Task<List<Tag>> GetAllTagsAsync();
        Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds);

        Task<List<Category>> GetAllCategoriesAsync();
        Task<List<SystemAccount>> GetAllSystemAccountsAsync();
        Task<SystemAccount> GetUserById(int id);

        Task<int> GetMaxNewsArticleIdAsync();
    }
}

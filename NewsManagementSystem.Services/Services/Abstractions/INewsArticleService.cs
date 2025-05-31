using NewManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.Services.Services.Abstractions
{
    public interface INewsArticleService
    {
        Task<List<NewsArticle>> GetAllAsync();
        Task<NewsArticle?> GetByIdAsync(int id);
        Task CreateAsync(NewsArticle article);
        Task UpdateAsync(NewsArticle article);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> GetMaxNewsArticleIdAsync();

        Task<List<Tag>> GetAllTagsAsync();
        Task<List<Category>> GetAllCategoriesAsync();
        Task<List<SystemAccount>> GetAllSystemAccountsAsync();
        Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds);
    }
}

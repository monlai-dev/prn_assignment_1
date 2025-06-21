using NewManagementSystem.Models;
using NewsManagementSystem.DataAccess.Repository.Abstractions;
using NewsManagementSystem.Services.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace NewsManagementSystem.Services.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _repo;

        public NewsArticleService(INewsArticleRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<NewsArticle>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<NewsArticle> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task CreateAsync(NewsArticle article) => await _repo.CreateAsync(article);
        public async Task UpdateAsync(NewsArticle article) => await _repo.UpdateAsync(article);
        public async Task DeleteAsync(int id) => await _repo.SoftDeleteAsync(id);

        public async Task<List<Tag>> GetAllTagsAsync() => await _repo.GetAllTagsAsync();
        public async Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds) => await _repo.GetTagsByIdsAsync(tagIds);
        public async Task<List<Category>> GetAllCategoriesAsync() => await _repo.GetAllCategoriesAsync();
        public async Task<List<SystemAccount>> GetAllSystemAccountsAsync() => await _repo.GetAllSystemAccountsAsync();
        public async Task<SystemAccount> GetUserById(int id) => await _repo.GetUserById(id);
        public async Task<int> GetMaxNewsArticleIdAsync() => await _repo.GetMaxNewsArticleIdAsync();
    }

}

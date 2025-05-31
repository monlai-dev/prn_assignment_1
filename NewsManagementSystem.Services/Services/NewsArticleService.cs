using Microsoft.EntityFrameworkCore;
using NewManagementSystem.Models;
using NewsManagementSystem.DataAccess.Repository.Abstractions;
using NewsManagementSystem.Services.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.Services.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _repository;

        public NewsArticleService(INewsArticleRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> GetMaxNewsArticleIdAsync() => await _repository.GetMaxNewsArticleIdAsync();

        public async Task<List<NewsArticle>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<NewsArticle?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task CreateAsync(NewsArticle article) => await _repository.CreateAsync(article);

        public async Task UpdateAsync(NewsArticle article) => await _repository.UpdateAsync(article);

        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);

        public async Task<bool> ExistsAsync(int id) => await _repository.ExistsAsync(id);

        public async Task<List<Tag>> GetAllTagsAsync() => await _repository.GetAllTagsAsync();

        public async Task<List<Category>> GetAllCategoriesAsync() => await _repository.GetAllCategoriesAsync();

        public async Task<List<SystemAccount>> GetAllSystemAccountsAsync() => await _repository.GetAllSystemAccountsAsync();

        public async Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds) => await _repository.GetTagsByIdsAsync(tagIds);
    }
}

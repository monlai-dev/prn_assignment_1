using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.BusinessObject.Models;
using NewManagementSystem.Models;
using NewManagementSystem.Repository.Abstractions;
using NewsManagementSystem.BusinessObject.Models;
using NewsManagementSystem.DataAccess.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.DataAccess.Repository
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly FunewsManagementContext _context;

        public NewsArticleRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public async Task<List<NewsArticle>> GetAllAsync()
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .Include(n => n.CreatedBy)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<NewsArticle> GetByIdAsync(int id)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .Include(n => n.CreatedBy)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id.ToString());
        }

        public async Task CreateAsync(NewsArticle article)
        {
            _context.NewsArticles.Add(article);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NewsArticle article)
        {
            _context.NewsArticles.Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var article = await _context.NewsArticles.FindAsync(id.ToString());
            if (article != null)
            {
                article.NewsStatus = false;
                _context.NewsArticles.Update(article);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds)
        {
            return await _context.Tags
                .Where(t => tagIds.Contains(t.TagId))
                .ToListAsync();
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<SystemAccount>> GetAllSystemAccountsAsync()
        {
            return await _context.SystemAccounts.ToListAsync();
        }

        public async Task<SystemAccount> GetUserById(int id)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(a => a.AccountId == id);
        }

        public async Task<int> GetMaxNewsArticleIdAsync()
        {
            var max = await _context.NewsArticles
                .Select(a => a.NewsArticleId)
                .ToListAsync();

            return max
                .Select(id => int.TryParse(id, out var num) ? num : 0)
                .DefaultIfEmpty(0)
                .Max();
        }
    }
}
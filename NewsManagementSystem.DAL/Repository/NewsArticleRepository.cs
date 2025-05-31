using Microsoft.EntityFrameworkCore;
using NewManagementSystem.Models;
using NewManagementSystem.Repository.Abstractions;
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
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .ToListAsync();
        }

        public async Task<int> GetMaxNewsArticleIdAsync()
        {
            var ids = await _context.NewsArticles
                .Select(n => n.NewsArticleId)
                .ToListAsync();

            var maxId = ids
                .Select(id => int.TryParse(id, out int parsed) ? parsed : 0)
                .DefaultIfEmpty(0)
                .Max();

            return maxId;
        }

        public async Task<NewsArticle?> GetByIdAsync(int id)
        {
            string idStr = id.ToString();
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.NewsArticleId == idStr);
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

        public async Task DeleteAsync(int id)
        {
            string idStr = id.ToString();
            var article = await _context.NewsArticles.FirstOrDefaultAsync(a => a.NewsArticleId == idStr);
            if (article != null)
            {
                _context.NewsArticles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            string idStr = id.ToString();
            return await _context.NewsArticles.AnyAsync(a => a.NewsArticleId == idStr);
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<SystemAccount>> GetAllSystemAccountsAsync()
        {
            return await _context.SystemAccounts.ToListAsync();
        }

        public async Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds)
        {
            return await _context.Tags
                .Where(t => tagIds.Contains(t.TagId))
                .ToListAsync();
        }

    }
}

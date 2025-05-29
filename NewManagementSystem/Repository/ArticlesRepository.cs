using Microsoft.EntityFrameworkCore;
using NewManagementSystem.Data;
using NewManagementSystem.Models;
using NewManagementSystem.Repository.Abstractions;

namespace NewManagementSystem.Repository
{
    public class ArticlesRepository : Repository<NewsArticle>, IArticleRepository
    {
        private readonly FunewsManagementContext _context;

        public ArticlesRepository(FunewsManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task<NewsArticle?> FindByStartAndEndDate(DateTime? startTime, DateTime? endTime, CancellationToken cancellationToken = default)
        {
            return await _context.NewsArticles
                .Where(article => article.CreatedDate >= startTime && article.CreatedDate <= endTime)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }

}

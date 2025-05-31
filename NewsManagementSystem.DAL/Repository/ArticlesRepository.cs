using Microsoft.EntityFrameworkCore;
using NewManagementSystem.Models;
using NewManagementSystem.Repository.Abstractions;
using NewsManagementSystem.DataAccess;

namespace NewManagementSystem.Repository;

public class ArticlesRepository : Repository<NewsArticle>, IArticleRepository
{
    private readonly FunewsManagementContext _context;

    public ArticlesRepository(FunewsManagementContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<IEnumerable<NewsArticle>> FindBetweenStartAndEndDateTime(DateTime start, DateTime end, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate input parameters
            if (start > end)
                throw new ArgumentException("Start date cannot be greater than end date", nameof(start));

            return await _context.NewsArticles
                .Where(article => article.CreatedDate >= start && article.CreatedDate <= end)
                .OrderByDescending(article => article.CreatedDate)
                .AsNoTracking() // Performance optimization for read-only operations
                .ToListAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw; // Re-throw cancellation exceptions
        }
        catch (Exception ex)
        {
            // Log the exception here in production
            throw new InvalidOperationException("An error occurred while retrieving articles", ex);
        }
    }
}
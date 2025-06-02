using NewManagementSystem.Models;
using NewManagementSystem.Repository.Abstractions;
using NewManagementSystem.Services.Abstractions;

namespace NewsManagementSystem.Services.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;

    public ArticleService(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
    }

    public async Task<IEnumerable<NewsArticle>> FindBetweenStartAndEndDateTime(DateTime start, DateTime end, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate business rules
            if (start > end)
                throw new ArgumentException("Start date cannot be greater than end date");

            var articles = await _articleRepository.FindBetweenStartAndEndDateTime(start, end, cancellationToken);
            
            return articles ?? Enumerable.Empty<NewsArticle>();
        }
        catch (ArgumentException)
        {
            throw; // Re-throw validation exceptions
            
            //test github bot
        }
        catch (OperationCanceledException)
        {
            throw; // Re-throw cancellation exceptions
        }
        catch (Exception ex)
        {
            // Log the exception here in production
            throw new InvalidOperationException("An error occurred while processing the article request", ex);
        }
    }
}
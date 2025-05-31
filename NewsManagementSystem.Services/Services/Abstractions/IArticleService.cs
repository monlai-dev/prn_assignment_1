using NewManagementSystem.Models;

namespace NewManagementSystem.Services.Abstractions;

public interface IArticleService
{
    Task<IEnumerable<NewsArticle>> FindBetweenStartAndEndDateTime(DateTime start, DateTime end, CancellationToken cancellationToken = default);
}
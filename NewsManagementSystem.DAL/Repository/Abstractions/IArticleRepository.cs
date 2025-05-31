using NewManagementSystem.Models;

namespace NewManagementSystem.Repository.Abstractions;

public interface IArticleRepository : IRepository<NewsArticle>
{
    Task<IEnumerable<NewsArticle>> FindBetweenStartAndEndDateTime(DateTime start, DateTime end, CancellationToken cancellationToken = default);
}
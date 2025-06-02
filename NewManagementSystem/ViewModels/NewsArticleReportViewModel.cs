namespace NewsManagementSystem.BusinessObject.ModelsDTO;

public class NewsReportStatsDto
{
    public int TotalArticles { get; set; }
    public int ActiveArticles { get; set; }
    public int InactiveArticles { get; set; }
    public Dictionary<string, int> ArticlesByCategory { get; set; } = new();
    public Dictionary<DateTime, int> ArticlesByDay { get; set; } = new();
    public string MostActiveAuthor { get; set; } = string.Empty;

    // 🔽 Add this:
    public List<NewsArticleDetailDto> Articles { get; set; } = new();
}

public class NewsArticleDetailDto
{
    public string Title { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string Author { get; set; } = "Unknown";
    public DateTime? CreatedDate { get; set; }
    public bool IsActive { get; set; }
}
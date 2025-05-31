namespace NewsManagementSystem.BusinessObject.ModelsDTO;

public class NewsArticleReportViewModel
{
    public string NewsArticleId { get; set; }
    public string? NewsTitle { get; set; }
    public string Headline { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? NewsSource { get; set; }
    public string? CategoryName { get; set; }
}
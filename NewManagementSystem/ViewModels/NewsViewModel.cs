namespace NewsManagementSystem.WebMVC.ViewModels
{
	public class NewsViewModel
	{
		public int? NewsArticleID { get; set; }
		public string? NewsTitle { get; set; }
		public string? Headline { get; set; }
		public DateTime CreatedDate { get; set; }
		public string? CategoryName { get; set; }
		public string? AuthorName { get; set; }
		public List<string>? Tags { get; set; }
	}
}

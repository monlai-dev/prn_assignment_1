namespace NewsManagementSystem.BusinessObject.Models
{
    public class ArticlesModel
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
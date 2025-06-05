namespace NewsManagementSystem.BusinessObject.Common
{
    public class PagedViewModel<TModel> where TModel : class
    {
        public IEnumerable<TModel> Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}

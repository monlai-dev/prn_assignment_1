
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NewsManagementSystem.BusinessObject.Models;

namespace NewsManagementSystem.BusinessObject.Models;

public partial class Tag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int TagId { get; set; }

    public string? TagName { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();

}

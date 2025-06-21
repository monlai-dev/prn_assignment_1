using System.ComponentModel.DataAnnotations;

namespace NewsManagementSystem.Web.ViewModels.TagViewModels
{
    public class TagViewModel
    {
        public int TagId { get; set; }

        [Required]
        [Display(Name = "Tag Name")]
        public string TagName { get; set; }

        public string Note { get; set; }

        public string FormAction { get; set; }
    }
}

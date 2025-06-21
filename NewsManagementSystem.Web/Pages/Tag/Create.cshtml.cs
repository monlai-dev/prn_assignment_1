using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewsManagementSystem.DataAccess;

namespace NewsManagementSystem.Web.Pages.Tag
{
    public class CreateModel : PageModel
    {
        private readonly FunewsManagementContext _context;

        public CreateModel(FunewsManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BusinessObject.Models.Tag Tag { get; set; } = default!;

        public IActionResult OnGet() => NotFound(); 

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    success = false,
                    errors = ModelState
                        .Where(kvp => kvp.Value.Errors.Any())
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        )
                });
            }

            _context.Tags.Add(Tag);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                success = true,
                tagId = Tag.TagId,
                tagName = Tag.TagName,
                note = Tag.Note
            });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NewsManagementSystem.Services.Services.Abstractions;



namespace NewsManagementSystem.Web.Pages
{
    public class TagsModel : PageModel
    {
        private readonly ITagService _tagService;
        private readonly IHubContext<DataHub> _hubContext;

        public TagsModel(ITagService tagService, IHubContext<DataHub> hubContext)
        {
            _tagService = tagService;
            _hubContext = hubContext;
        }

        public IList<BusinessObject.Models.Tag> Tags { get; set; } = new List<BusinessObject.Models.Tag>();

        [BindProperty]
        public BusinessObject.Models.Tag TagInput { get; set; } = new BusinessObject.Models.Tag();

        [BindProperty]
        public int DeleteTagId { get; set; }

        public async Task OnGetAsync()
        {
            Tags = await _tagService.GetAllTagsAsync();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                Tags = await _tagService.GetAllTagsAsync();
                return Page();
            }
            var createdTag = await _tagService.CreateTagAsync(TagInput);
            await _hubContext.Clients.All.SendAsync("ReceiveTagUpdate", "create", createdTag);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                Tags = await _tagService.GetAllTagsAsync();
                return Page();
            }
            await _tagService.UpdateTagAsync(TagInput);
            await _hubContext.Clients.All.SendAsync("ReceiveTagUpdate", "edit", TagInput);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            await _tagService.DeleteTagAsync(DeleteTagId);
            await _hubContext.Clients.All.SendAsync("ReceiveTagUpdate", "delete", DeleteTagId);
            return RedirectToPage();
        }

        public async Task<JsonResult> OnGetTagAsync(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            return new JsonResult(tag);
        }
    }
}

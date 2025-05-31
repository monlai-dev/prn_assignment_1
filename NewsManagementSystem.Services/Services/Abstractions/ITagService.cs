using NewManagementSystem.Models;

namespace NewsManagementSystem.Services.Services.Abstractions
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllTagsAsync();
        Task<Tag> GetTagByIdAsync(int id);
        Task<Tag> CreateTagAsync(Tag tag);
        Task UpdateTagAsync(Tag tag);
        Task DeleteTagAsync(int id);
        Task<bool> IsTagInUseAsync(int id);



    }
}

using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NewManagementSystem.Models;
using NewsManagementSystem.DataAccess.Repository.Abstractions;
using NewsManagementSystem.Services.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.Services.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<Tag> CreateTagAsync(Tag tag)
        {
            int maxId = await _tagRepository.GetMaxTagIdAsync();
            int nextId = maxId + 1;

            var newTag = new Tag
            {
                TagId = nextId,
                TagName = tag.TagName,
                Note = tag.Note
            };

            Console.WriteLine("Tạo Tag mới với ID: " + newTag.TagId);
            await _tagRepository.AddAsync(newTag);
            return newTag;
        }

        public async Task<List<Tag>> GetAllTagsAsync() => await _tagRepository.GetAllAsync();
        public async Task<Tag> GetTagByIdAsync(int id) => await _tagRepository.GetByIdAsync(id);
        public async Task UpdateTagAsync(Tag tag) => await _tagRepository.UpdateAsync(tag);
        public async Task DeleteTagAsync(int id) => await _tagRepository.DeleteAsync(id);
        public async Task<bool> IsTagInUseAsync(int id) => await _tagRepository.IsTagInUseAsync(id);


    }
}

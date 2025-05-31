using NewManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.DataAccess.Repository.Abstractions
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetAllAsync();
        Task<Tag> GetByIdAsync(int id);
        Task AddAsync(Tag tag);
        Task UpdateAsync(Tag tag);
        Task DeleteAsync(int id);
        Task<bool> IsTagInUseAsync(int tagId);
        Task<int> GetMaxTagIdAsync();
    }
}

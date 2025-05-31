using NewManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.DataAccess.Repository.Abstractions
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        Category? GetById(short id);
        void Add(Category category);
        void Update(Category category);
        bool Delete(short id);
        bool IsCategoryUsed(short id);
    }
}

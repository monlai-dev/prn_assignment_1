using NewManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.Services.Services.Abstractions
{
    public interface ICategoryServices
    {
        IEnumerable<Category> GetAll();
        Category? GetById(short id);
        void Add(Category category);
        void Update(Category category);
        bool Delete(short id);
    }
}

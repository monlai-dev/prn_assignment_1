using NewManagementSystem.Models;
using NewsManagementSystem.DataAccess.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.DataAccess.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FunewsManagementContext _context;

        public CategoryRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.Where(c => c.IsActive == true).ToList();
        }

        public Category? GetById(short id)
        {
            return _context.Categories.Find(id);
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public bool Delete(short id)
        {
            var category = _context.Categories.Find(id);
            if (category == null || IsCategoryUsed(id)) return false;

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return true;
        }

        public bool IsCategoryUsed(short id)
        {
            return _context.NewsArticles.Any(n => n.CategoryId == id);
        }
    }

}

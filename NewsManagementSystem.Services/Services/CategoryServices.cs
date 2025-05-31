using NewManagementSystem.Models;
using NewManagementSystem.Repository;
using NewManagementSystem.Repository.Abstractions;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.DataAccess.Repository.Abstractions;
using NewsManagementSystem.Services.Services.Abstractions;

namespace NewsManagementSystem.Services.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository _repository;

        public CategoryServices(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Category> GetAll() => _repository.GetAll();

        public Category? GetById(short id) => _repository.GetById(id);

        public void Add(Category category) => _repository.Add(category);

        public void Update(Category category) => _repository.Update(category);

        public bool Delete(short id)
        {
            // Check if the category is used in any NewsArticle
            if (_repository.IsCategoryUsed(id))
                return false;

            // Proceed to delete using the repository
            return _repository.Delete(id);
        }
    }
}

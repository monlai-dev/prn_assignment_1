using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewManagementSystem.Models;
using NewsManagementSystem.Services.Services.Abstractions;

namespace NewsManagementSystem.WebMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryServices _service;

        public CategoryController(ICategoryServices service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var categories = _service.GetAll();
            return View(categories);
        }

        public IActionResult Details(short id)
        {
            var category = _service.GetById(id);
            if (category == null) return NotFound();
            return View(category);
        }

        public IActionResult Create()
        {
            var category = new Category(); // Initialize a new Category object
            ViewData["ParentCategoryId"] = new SelectList(_service.GetAll(), "CategoryId", "CategoryName");
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _service.Add(category);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentCategoryId"] = new SelectList(_service.GetAll(), "CategoryId", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        public IActionResult Edit(short id)
        {
            var category = _service.GetById(id);
            if (category == null) return NotFound();
            ViewData["ParentCategoryId"] = new SelectList(_service.GetAll(), "CategoryId", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _service.Update(category);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentCategoryId"] = new SelectList(_service.GetAll(), "CategoryId", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        public IActionResult Delete(short id)
        {
            var category = _service.GetById(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(short id)
        {
            bool success = _service.Delete(id);
            if (!success)
            {
                TempData["Error"] = "Cannot delete this category. It is being used in news articles.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

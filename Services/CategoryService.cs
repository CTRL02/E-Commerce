using E_Commerce.Context;
using E_Commerce.DTO;
using E_Commerce.Interfaces;
using E_Commerce.Models;
using Google;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ECommerceDbContext _context;

        public CategoryService(ECommerceDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.CategoryId,
                    Name = c.CategoryName,
                    ParentCategoryId = c.ParentCategoryId,
                })
                .ToListAsync();

            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            if (category == null)
            {
                throw new ValidationException("Category not found.");
            }

            return category;
        }
    }
}

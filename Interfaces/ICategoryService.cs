using E_Commerce.DTO;
using E_Commerce.Models;

namespace E_Commerce.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        public Task<Category> GetCategoryByIdAsync(int categoryId);

        //Task<IEnumerable<Category>> GetCategoriesWithSubCategoriesAsync();
    }
}

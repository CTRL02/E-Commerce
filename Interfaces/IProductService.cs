using E_Commerce.DTO;
using E_Commerce.Models;

namespace E_Commerce.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductItem>> SearchProductsAsync(string keyword);

        public Task<bool> AddProductAsync(Product product);

        Task AddReviewAsync(Review review);

        public Task<Product?> GetProductByNormalizedNameAsync(string normalizedProductName);

        public Task<bool> AddProductItemAsync(ProductItem productItem);

        public Task<bool> UpdateProductItemAsync(ProductItem productItem);

        public Task<ProductItem> GetProductItemByIdAsync(int productItemId);

        public Task<IEnumerable<ProductItem>> GetProductItemsByCategoryAsync(int categoryId);

        public Task<IEnumerable<ProductItemDTO>> GetTopRatedProductsAsync();

    }
}

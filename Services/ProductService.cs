using E_Commerce.Context;
using E_Commerce.DTO;
using E_Commerce.Interfaces;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Services
{
    public class ProductService : IProductService
    {
        private readonly ECommerceDbContext _context;

        public ProductService(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductItem>> SearchProductsAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                throw new ArgumentException("Keyword cannot be null or empty.", nameof(keyword));
            }

            return await _context.ProductItems
                .Include(pi => pi.Product)
                .Where(pi => pi.Product.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                             pi.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                             pi.Product.Category.CategoryName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                             pi.Product.Category.ParentCategory.CategoryName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            if (string.IsNullOrEmpty(product.Name))
            {
                throw new ValidationException("Product name is required.");
            }

            if (product.CategoryId <= 0)
            {
                throw new ValidationException("Invalid category ID.");
            }

            if (product.ProductItems == null || product.ProductItems.Count == 0)
            {
                throw new ValidationException("Product must have at least one product item.");
            }

            foreach (var item in product.ProductItems)
            {
                if (string.IsNullOrEmpty(item.SKU))
                {
                    throw new ValidationException("SKU is required for each product item.");
                }

                if (item.Price <= 0)
                {
                    throw new ValidationException("Price must be greater than zero.");
                }

                if (item.QtyInStock < 0)
                {
                    throw new ValidationException("Quantity in stock cannot be negative.");
                }
            }

            try
            {
                await _context.Products.AddAsync(product);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task AddReviewAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }
        public async Task<Product?> GetProductByNormalizedNameAsync(string normalizedProductName)
        {
            try
            {
                return await _context.Products
                    .Include(p => p.ProductItems) 
                    .FirstOrDefaultAsync(p => p.Name == normalizedProductName);
            }
            catch (Exception ex)
            {
                return null; 
            }
        }

        public async Task<bool> AddProductItemAsync(ProductItem productItem)
        {
            try
            {
                _context.ProductItems.Add(productItem);

                var result = await _context.SaveChangesAsync();

                return result > 0; 
            }
            catch (Exception ex)
            {
                return false; 
            }
        }

        public async Task<bool> UpdateProductItemAsync(ProductItem productItem)
        {
            try
            {
                _context.ProductItems.Update(productItem);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ProductItem?> GetProductItemByIdAsync(int productItemId)
        {
            return await _context.ProductItems
                .Include(pi => pi.Product) // Include related product details
                .Include(pi => pi.Seller) // Optionally include the seller
                .FirstOrDefaultAsync(pi => pi.Id == productItemId);
        }

        public async Task<IEnumerable<ProductItemDTO>> GetTopRatedProductsAsync()
        {
            var items = await _context.ProductItems
                .Include(p => p.OrderLines)
                .ThenInclude(ol => ol.Reviews)
                .Where(p => p.OrderLines.Any(ol => ol.Reviews.Any()))
                .Select(p => new ProductItemDTO
                {
                    Id = p.Id,
                    Name = p.Product.Name,
                    Price = p.Price,
                    Description = p.Description,
                    ProductImage = p.ProductImage,
                    AverageRating = p.OrderLines
                                      .SelectMany(ol => ol.Reviews)
                                      .Average(r => (double?)r.RatingValue) ?? 0
                })
                .OrderByDescending(p => p.AverageRating)
                .ToListAsync();

            return items;
        }

        public async Task<IEnumerable<ProductItem>> GetProductItemsByCategoryAsync(int categoryId)
        {

            var items = await _context.ProductItems
                                      .Where(pi => pi.Product.CategoryId == categoryId)
                                      .OrderBy(pi => pi.Id)
                                      .ToListAsync();

            return items;
        }

    }
}

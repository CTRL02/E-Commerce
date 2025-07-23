using E_Commerce.DTO;
using E_Commerce.Interfaces;
using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_Commerce.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class SellerProductController : ControllerBase
    {
        private readonly ISellerService _SellerService;
        private readonly ICategoryService _CategoryService;
        private readonly IVariationService _VariationService;
        private readonly IProductService _ProductService;

        public SellerProductController(ISellerService sellerService, ICategoryService categoryService, 
            IVariationService variationService, IProductService productService)
        {
            _SellerService = sellerService;
            _CategoryService = categoryService;
            _VariationService = variationService;
            _ProductService = productService;
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _CategoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("GetVariations/{categoryId}")]
        public async Task<IActionResult> GetVariations(int categoryId)
        {
            var variations = await _VariationService.GetVariationsByCategoryAsync(categoryId);
            return Ok(variations);
        }

        [HttpGet("GetVariationOptions/{variationId}")]
        public async Task<IActionResult> GetVariationOptions(int variationId)
        {
            var variationOptions = await _VariationService.GetOptionsByVariationAsync(variationId);
            return Ok(variationOptions);
        }

        [Authorize(Roles = "Seller, Admin")]
        [HttpPost("AddProductItem")]
        public async Task<IActionResult> AddProductItem([FromBody] AddProductRequestDTO productRequest)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(sellerId))
            {
                return Unauthorized("Seller not identified.");
            }

            var category = await _CategoryService.GetCategoryByIdAsync(productRequest.CategoryId);
            if (category == null)
            {
                return BadRequest("Invalid category.");
            }

            var normalizedProductName = productRequest.Name.ToLower().Replace(" ", "");

            var existingProduct = await _ProductService.GetProductByNormalizedNameAsync(normalizedProductName);

            Product product;
            if (existingProduct != null)
            {
                product = existingProduct;
            }
            else
            {
                product = new Product
                {
                    Name = normalizedProductName,
                    CategoryId = productRequest.CategoryId,
                    ProductItems = new List<ProductItem>()
                };

                var productAdded = await _ProductService.AddProductAsync(product);
                if (!productAdded)
                {
                    return StatusCode(500, "Error occurred while creating the product.");
                }
            }

            var productItem = new ProductItem
            {
                SKU = productRequest.SKU,
                Price = productRequest.Price,
                QtyInStock = productRequest.QtyInStock,
                Description = productRequest.Description,
                ProductImage = productRequest.ProductImage,
                SellerId = sellerId,
            };

            var productItemAdded = await _ProductService.AddProductItemAsync(productItem);
            if (!productItemAdded)
            {
                return StatusCode(500, "Error occurred while saving the product item.");
            }

            foreach (var variation in productRequest.Variations)
            {
                var variationEntity = await _VariationService.GetVariationByIdAsync(variation.VariationId, productRequest.CategoryId);
                if (variationEntity == null || variationEntity.CategoryId != productRequest.CategoryId)
                {
                    return BadRequest($"Invalid variation for category: {variation.VariationId}");
                }

                var optionEntity = await _VariationService.GetVariationOptionByIdAsync(variation.VariationOptionId, variation.VariationId);
                if (optionEntity == null || optionEntity.VariationId != variation.VariationId)
                {
                    return BadRequest($"Invalid variation option: {variation.VariationOptionId}");
                }

                var productConfiguration = new ProductConfiguration
                {
                    VariationOptionId = variation.VariationOptionId,
                    ProductItemId = productItem.Id 
                };

                productItem.ProductConfigurations.Add(productConfiguration);
            }

            var productConfigurationsAdded = await _ProductService.UpdateProductItemAsync(productItem);
            if (!productConfigurationsAdded)
            {
                return StatusCode(500, "Error occurred while saving product configurations.");
            }

            return Ok("Product item and configurations added successfully.");
        }

        [Authorize(Roles = "Seller, Admin")]
        [HttpGet("GetProductItems/{sellerId}")]
        public async Task<IActionResult> GetProductItems(string sellerId)
        {
            var realSellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (realSellerId == sellerId)
            {
                var productItems = await _SellerService.GetProductItemsBySellerAsync(sellerId);
                if (productItems == null || !productItems.Any())
                {
                    return NotFound("No product items found for this seller.");
                }

                return Ok(productItems);
            }
            else
            {
                return Forbid("Not Authorized");
            }

        }

        [Authorize(Roles = "Seller, Admin")]
        [HttpDelete("DeleteProductItem/{productItemId}")]
        public async Task<IActionResult> DeleteProductItem(int productItemId)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var productItem = await _SellerService.DeleteProductItemAsync(productItemId, sellerId);

            if (productItem == null)
            {
                return NotFound("Product item not found or you do not have permission to delete this item.");
            }

            return Ok("Product item deleted successfully.");
        }

        [Authorize(Roles = "Seller, Admin")]
        [HttpPut("EditProductItem/{productItemId}")]
        public async Task<IActionResult> EditProductItem(int productItemId, [FromBody] EditProductDTO productItemDTO)
        {
            if (productItemDTO == null)
            {
                return BadRequest("Invalid product data.");
            }
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updatedProductItem = await _SellerService.EditProductItemAsync(productItemDTO, sellerId, productItemId);

            if (updatedProductItem == null)
            {
                return NotFound("Product item not found or you do not have permission to edit this item.");
            }

            return Ok(updatedProductItem);
        }

        private string GenerateSKU(int productId, int variationId)
        {
            return $"SKU-{productId}-{variationId}";
        }
    }
}

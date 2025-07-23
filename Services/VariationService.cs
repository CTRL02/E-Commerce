using E_Commerce.Context;
using E_Commerce.Interfaces;
using E_Commerce.Models;
using Google;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Services
{
    public class VariationService : IVariationService
    {
        private readonly ECommerceDbContext _dbContext;

        public VariationService(ECommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Variation>> GetVariationsByCategoryAsync(int categoryId)
        {
            return await _dbContext.Variations
                .Where(v => v.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<Variation> GetVariationByIdAsync(int variationId, int categoryId)
        {
            var variation = await _dbContext.Variations
                .FirstOrDefaultAsync(v => v.Id == variationId);

            if (variation == null)
            {
                throw new ValidationException("Variation not found.");
            }

            if (variation.CategoryId != categoryId)
            {
                throw new ValidationException($"Variation does not belong to the category with ID {categoryId}.");
            }

            return variation;
        }

        public async Task<VariationOption> GetVariationOptionByIdAsync(int variationOptionId, int variationId)
        {
            var variationOption = await _dbContext.VariationOptions
                .FirstOrDefaultAsync(vo => vo.Id == variationOptionId);

            if (variationOption == null)
            {
                throw new ValidationException("Variation option not found.");
            }

            if (variationOption.VariationId != variationId)
            {
                throw new ValidationException($"Variation option does not belong to the variation with ID {variationId}.");
            }

            return variationOption;
        }

        public async Task<IEnumerable<VariationOption>> GetOptionsByVariationAsync(int variationId)
        {
            return await _dbContext.VariationOptions
                .Where(vo => vo.VariationId == variationId)
                .ToListAsync();
        }

    }
}

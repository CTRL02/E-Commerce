using E_Commerce.DTO;
using E_Commerce.Models;

namespace E_Commerce.Interfaces
{
    public interface IVariationService
    {
        Task<List<Variation>> GetVariationsByCategoryAsync(int categoryId);
        Task<IEnumerable<VariationOption>> GetOptionsByVariationAsync(int variationId);

        public Task<Variation> GetVariationByIdAsync(int variationId, int categoryId);
        public Task<VariationOption> GetVariationOptionByIdAsync(int variationOptionId, int variationId);

    }
}

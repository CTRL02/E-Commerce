using E_Commerce.Services;

namespace E_Commerce.Interfaces
{
    public interface IPaginationService
    {
        Task<PaginatedResult<T>> GetPaginatedResultAsync<T>(IEnumerable<T> source, int pageNumber, int pageSize);
    }
}

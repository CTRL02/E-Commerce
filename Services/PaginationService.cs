using E_Commerce.Interfaces;
using System;
using System.Collections.Generic;

namespace E_Commerce.Services
{
    public class PaginationService<T> : IPaginationService
    {
        public async Task<PaginatedResult<T1>> GetPaginatedResultAsync<T1>(IEnumerable<T1> source, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) throw new ArgumentException("Page number should be greater than 0.");
            if (pageSize < 1) throw new ArgumentException("Page size should be greater than 0.");

            var totalItems = source.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var data = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var result = new PaginatedResult<T1>
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                Data = data
            };

            return await Task.FromResult(result);
        }
    }

    public class PaginatedResult<T>
    {
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}

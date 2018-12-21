﻿using System.Threading.Tasks;

namespace MelonPay.Abstractions
{
    public interface ICatalogueRepository<T>
    {
        Task<T[]> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T model);
        Task<T> UpdateAsync(T model);
        Task DeleteAsync(int id);
        Task DeleteAsync(T model);
    }
}

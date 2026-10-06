using System;
using System.Collections.Generic;
using System.Text;

namespace FeyzaStore.Application.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);

    Task SaveChangesAsync();
}
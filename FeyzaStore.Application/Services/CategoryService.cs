using FeyzaStore.Application.Interfaces;
using FeyzaStore.Domain.Entities;

namespace FeyzaStore.Application.Services;

public class CategoryService
{
    private readonly IGenericRepository<Category> _repository;

    public CategoryService(IGenericRepository<Category> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(Category category)
    {
        await _repository.AddAsync(category);
        await _repository.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Category category)
    {
        var existingCategory = await _repository.GetByIdAsync(category.Id);

        if (existingCategory == null)
            return false;

        existingCategory.Name = category.Name;

        _repository.Update(existingCategory);
        await _repository.SaveChangesAsync();

        return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);

        if (category == null)
            return false;

        _repository.Delete(category);
        await _repository.SaveChangesAsync();

        return true;
    }
}
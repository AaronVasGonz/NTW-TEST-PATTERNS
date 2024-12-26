using Data.Repository.Data.Repository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public interface ICategoryService
{
    Task<bool> DeleteCategoryAsync(int id);
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<Category> GetCategoryByIdAsync(int id);
    Task<Category> SaveCategoryAsync(Category category);
}

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        return await _categoryRepository.GetCategoriesAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _categoryRepository.GetCategoryByIdAsync(id);
    }

    public async Task<Category> SaveCategoryAsync(Category category)
    {
        return await _categoryRepository.SaveCategoryAsync(category);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        return await _categoryRepository.DeleteCategoryAsync(id);
    }
}


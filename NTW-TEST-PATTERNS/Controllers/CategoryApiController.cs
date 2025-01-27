using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOS;
using Service.Services;
using Service.Services.validations.Categories;
using Services;

namespace NTW_TEST_PATTERNS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryApiController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidateCategoryService _validateCategoryService;
        public CategoryApiController(ICategoryService categoryService, IValidateCategoryService validateCategoryService)
        {
            _categoryService = categoryService;
            _validateCategoryService = validateCategoryService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return categories == null ? throw new KeyNotFoundException("Categories not found)") : (IActionResult)Ok(categories);
        }

        [HttpGet("category/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return category == null ? throw new KeyNotFoundException("Category not found") : (IActionResult)Ok(category);
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveCategory([FromBody] CategoryRequest categoryRequest)
        {
            if (categoryRequest == null)
            {
                throw new ArgumentNullException("Category is null");
            }

            //validate the category request
            _validateCategoryService.ValidateCategoryRequest(categoryRequest);

            var category = new Category
            {
                CategoryName = categoryRequest.CategoryName,
                Description = categoryRequest.Description,
                Status = categoryRequest.Status
            };

            var savedCategory = await _categoryService.SaveCategoryAsync(category);
            if (savedCategory == null)
                throw new Exception("Failed to save category");
            return Ok(new { message = " Category saved successfully" });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryRequest categoryRequest)
        {
            if (categoryRequest == null)
            {
                throw new ArgumentNullException("Category is null");
            }
            //validate the category request
            _validateCategoryService.ValidateCategoryRequest(categoryRequest);
            var category = new Category
            {
                CategoryId = categoryRequest.Id,
                CategoryName = categoryRequest.CategoryName,
                Description = categoryRequest.Description,
                Status = categoryRequest.Status
            };
            var updatedCategory = await _categoryService.SaveCategoryAsync(category);
            if (updatedCategory == null)
                throw new Exception("Failed to update category");

            return Ok(new { message = "Category updated successfully" });
        }
    }
}


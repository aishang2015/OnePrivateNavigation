using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnePrivateNavigation.Common.Models;
using OnePrivateNavigation.Common.Models.Category;
using OnePrivateNavigation.Data;
using OnePrivateNavigation.Data.Entities;

namespace OnePrivateNavigation.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly OnePrivateNavigationDbContext _context;

        public CategoryController(OnePrivateNavigationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CategoryResponse>>>> GetCategories()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.DisplayOrder)
                .Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    DisplayOrder = c.DisplayOrder,
                    IsVisible = c.IsVisible
                })
                .ToListAsync();

            return ApiResponse<List<CategoryResponse>>.Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(ApiResponse<CategoryResponse>.Error("分组不存在"));
            }

            var response = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                DisplayOrder = category.DisplayOrder,
                IsVisible = category.IsVisible
            };

            return ApiResponse<CategoryResponse>.Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> CreateCategory(CategoryRequest request)
        {
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                DisplayOrder = request.DisplayOrder,
                IsVisible = request.IsVisible
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var response = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                DisplayOrder = category.DisplayOrder,
                IsVisible = category.IsVisible
            };

            return ApiResponse<CategoryResponse>.Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> UpdateCategory(int id, CategoryRequest request)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(ApiResponse<CategoryResponse>.Error("分组不存在"));
            }

            category.Name = request.Name;
            category.Description = request.Description;
            category.DisplayOrder = request.DisplayOrder;
            category.IsVisible = request.IsVisible;

            await _context.SaveChangesAsync();

            var response = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                DisplayOrder = category.DisplayOrder,
                IsVisible = category.IsVisible
            };

            return ApiResponse<CategoryResponse>.Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(ApiResponse<bool>.Error("分组不存在"));
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true);
        }
    }
}
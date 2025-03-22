using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnePrivateNavigation.Common.Models;
using OnePrivateNavigation.Common.Models.Website;
using OnePrivateNavigation.Data;
using OnePrivateNavigation.Data.Entities;
using OnePrivateNavigation.Helpers;

namespace OnePrivateNavigation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WebsiteController : ControllerBase
    {
        private readonly OnePrivateNavigationDbContext _context;
        private readonly FaviconHelper _faviconHelper;

        public WebsiteController(OnePrivateNavigationDbContext context, FaviconHelper faviconHelper)
        {
            _context = context;
            _faviconHelper = faviconHelper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<WebsiteResponse>>>> GetWebsites()
        {
            var websites = await _context.Websites
                .OrderBy(w => w.DisplayOrder)
                .Select(w => new WebsiteResponse
                {
                    Id = w.Id,
                    Title = w.Title,
                    Url = w.Url,
                    Description = w.Description,
                    Icon = w.Icon,
                    DisplayOrder = w.DisplayOrder,
                    IsVisible = w.IsVisible,
                    CategoryId = w.CategoryId
                })
                .ToListAsync();

            return ApiResponse<List<WebsiteResponse>>.Ok(websites);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WebsiteResponse>>> GetWebsite(int id)
        {
            var website = await _context.Websites.FindAsync(id);
            if (website == null)
            {
                return NotFound(ApiResponse<WebsiteResponse>.Error("导航网站不存在"));
            }

            var response = new WebsiteResponse
            {
                Id = website.Id,
                Title = website.Title,
                Url = website.Url,
                Description = website.Description,
                Icon = website.Icon,
                DisplayOrder = website.DisplayOrder,
                IsVisible = website.IsVisible,
                CategoryId = website.CategoryId
            };

            return ApiResponse<WebsiteResponse>.Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<WebsiteResponse>>> CreateWebsite(WebsiteRequest request)
        {
            var website = new Website
            {
                Title = request.Title,
                Url = request.Url,
                Description = request.Description,
                DisplayOrder = request.DisplayOrder,
                IsVisible = request.IsVisible,
                CategoryId = request.CategoryId
            };

            website.Icon = await _faviconHelper.GetAndSaveFaviconAsync(request.Url);

            _context.Websites.Add(website);
            await _context.SaveChangesAsync();

            var response = new WebsiteResponse
            {
                Id = website.Id,
                Title = website.Title,
                Url = website.Url,
                Description = website.Description,
                Icon = website.Icon,
                DisplayOrder = website.DisplayOrder,
                IsVisible = website.IsVisible,
                CategoryId = website.CategoryId
            };

            return ApiResponse<WebsiteResponse>.Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<WebsiteResponse>>> UpdateWebsite(int id, WebsiteRequest request)
        {
            var website = await _context.Websites.FindAsync(id);
            if (website == null)
            {
                return ApiResponse<WebsiteResponse>.Error("导航网站不存在");
            }

            website.Title = request.Title;
            website.Url = request.Url;
            website.Description = request.Description;
            website.DisplayOrder = request.DisplayOrder;
            website.IsVisible = request.IsVisible;
            website.CategoryId = request.CategoryId;

            website.Icon = await _faviconHelper.GetAndSaveFaviconAsync(request.Url);

            await _context.SaveChangesAsync();

            var response = new WebsiteResponse
            {
                Id = website.Id,
                Title = website.Title,
                Url = website.Url,
                Description = website.Description,
                Icon = website.Icon,
                DisplayOrder = website.DisplayOrder,
                IsVisible = website.IsVisible,
                CategoryId = website.CategoryId
            };

            return ApiResponse<WebsiteResponse>.Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteWebsite(int id)
        {
            var website = await _context.Websites.FindAsync(id);
            if (website == null)
            {
                return NotFound(ApiResponse<bool>.Error("导航网站不存在"));
            }

            _context.Websites.Remove(website);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true);
        }
    }
}
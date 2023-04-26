using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObbedNews.Data;
using ObbedNews.Dto.Categories;
using ObbedNews.Models;

namespace ObbedNews.Controllers.Categories;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IList<Category>>> GetCategories()
    {
        return await _context.Categories
            .OrderBy(c => c.Name)
            .Include(c => c.ParentCategory)
            .ToListAsync();
    }

    [HttpGet("GetParentCategories")]
    public async Task<ActionResult<IList<ParentCategory>>> GetParentCategories()
    {
        return await _context.ParentCategories
            .OrderBy(pc => pc.Name)
            .ToListAsync();
    }

    // POST: api/Categories/AddParentCategory
    [HttpPost("AddParentCategory")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddParentCategory(
        [FromBody] AddParentCategoryDto dto)
    {
        var pc = new ParentCategory
        {
            Name = dto.Name,
            Slug = dto.Slug
        };

        await _context.ParentCategories.AddAsync(pc);
        await _context.SaveChangesAsync();


        return Ok(new { pc.Id });
    }

    // POST: api/Categories
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddCategory(
        [FromBody] AddCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Slug = dto.Slug,
            ParentCategoryId = dto.ParentCategoryId
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();


        return Ok(new { category.Id });
    }
}
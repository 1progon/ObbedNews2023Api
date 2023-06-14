using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obbed.Data;
using Obbed.Dto.Homepage;
using Obbed.Enums.News.Comments;
using Obbed.Models.Words;

namespace Obbed.Controllers.Homepage;

[Route("api/[controller]")]
[ApiController]
public class HomepageController : ControllerBase
{
    private readonly AppDbContext _context;

    public HomepageController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<HomepageDto>> Homepage()
    {
        var popular = await _context.Words
            .OrderByDescending(m => m.CreatedAt)
            .Where(w => w.Popular && !w.IsDraft)
            .Select(w => new Word
            {
                Name = w.Name, Slug = w.Slug
            })
            .Skip(0)
            .Take(20)
            .ToListAsync();

        var mostCommented = await _context.Words
            .OrderByDescending(w => w.Comments.Count)
            .Where(w => w.Comments
                .Count(c => c.Status == CommentStatus.Active) > 9 && !w.IsDraft)
            .Select(w => new Word
            {
                Name = w.Name, Slug = w.Slug
            })
            .Skip(0)
            .Take(20)
            .ToListAsync();

        var last = await _context.Words
            .OrderByDescending(m => m.CreatedAt)
            .Where(w => !w.IsDraft)
            .Select(w => new Word
            {
                Name = w.Name, Slug = w.Slug
            })
            .Skip(0)
            .Take(20)
            .ToListAsync();


        return new HomepageDto
        {
            Popular = popular,
            MostCommented = mostCommented,
            Last = last
        };
    }
}
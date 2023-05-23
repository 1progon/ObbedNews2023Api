using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obbed.Data;
using Obbed.Dto.Homepage;
using Obbed.Enums.News.Comments;

namespace Obbed.Controllers.Homepage;

[Route("api/[controller]")]
[ApiController]
public class HomepageController : ControllerBase
{
    private readonly AppDbContext _context;

    public HomepageController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<HomepageDto>> Homepage()
    {
        // todo implement popular property
        var popular = await _context.Words
            .OrderByDescending(m => m.CreatedAt)
            .Where(m => m.Popular && !m.IsDraft)
            .Skip(0)
            .Take(20)
            .ToListAsync();

        var mostCommented = await _context.Words
            .Where(m => m.Comments
                .Count(c => c.Status == CommentStatus.Active) > 9 && !m.IsDraft)
            .OrderByDescending(m => m.Comments.Count)
            .Skip(0)
            .Take(20)
            .ToListAsync();

        var last = await _context.Words
            .OrderByDescending(m => m.CreatedAt)
            .Skip(0)
            .Take(20)
            .Where(w => !w.IsDraft)
            .ToListAsync();


        return new HomepageDto
        {
            Popular = popular,
            MostCommented = mostCommented,
            Last = last
        };
    }
}
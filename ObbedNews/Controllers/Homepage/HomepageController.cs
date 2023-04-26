using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObbedNews.Data;
using ObbedNews.Dto.Homepage;
using ObbedNews.Enums.News.Comments;

namespace ObbedNews.Controllers.Homepage;

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
        var popular = await _context.News
            .OrderByDescending(m => m.CreatedAt)
            .Where(m => m.Popular)
            .Skip(0)
            .Take(20)
            .ToListAsync();

        var mostCommented = await _context.News
            .Where(m => m.Comments
                .Count(c => c.Status == CommentStatus.Active) > 9)
            .OrderByDescending(m => m.Comments.Count)
            .Skip(0)
            .Take(20)
            .ToListAsync();

        var last = await _context.News
            .OrderByDescending(m => m.CreatedAt)
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obbed.Data;
using Obbed.Enums.News.Comments;
using Obbed.Models.Words;

namespace Obbed.Controllers.Comments
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context) => _context = context;

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WordComment>>> GetComments()
        {
            var comments = await _context.Comments
                .OrderByDescending(c => c.Id)
                .Include(c => c.User.Account)
                .Include(c => c.Word)
                .ToListAsync();

            return comments;
        }

        // GET: api/Comments/GetCommentsWaitModeration
        [HttpGet("GetCommentsWaitModeration")]
        public async Task<ActionResult<IEnumerable<WordComment>>> GetCommentsWaitModeration()
        {
            var comments = await _context.Comments
                .OrderByDescending(c => c.Id)
                .Include(c => c.User.Account)
                .Include(c => c.Word)
                .Where(c => c.Status == CommentStatus.Moderation)
                .ToListAsync();

            return comments;
        }

        // POST: api/Comments/565/UpdateCommentStatus
        [HttpPost("{commentId}/UpdateCommentStatus")]
        public async Task<ActionResult<WordComment>> UpdateCommentStatus(
            [FromRoute] long commentId, [FromQuery] CommentStatus commentStatus)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment is null) return NotFound();

            // todo update comment status
            comment.Status = commentStatus;

            await _context.SaveChangesAsync();

            return comment;
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WordComment>> GetComment(long id)
        {
            if (_context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(long id, WordComment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WordComment>> PostComment(WordComment comment)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'AppDbContext.Comments'  is null.");
            }

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(long id)
        {
            if (_context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(long id)
        {
            return (_context.Comments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
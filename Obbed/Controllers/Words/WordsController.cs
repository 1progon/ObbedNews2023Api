using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obbed.Data;
using Obbed.Dto.VideoServer;
using Obbed.Dto.Words;
using Obbed.Enums.News;
using Obbed.Enums.News.Comments;
using Obbed.Models;
using Obbed.Models.Middle;
using Obbed.Models.Words;

namespace Obbed.Controllers.Words;

[ApiController]
[Route("api/[controller]")]
public class WordsController : ControllerBase
{
    private const string ImagesSubDir = "words";

    private readonly string _absoluteImagesDir;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public WordsController(AppDbContext context,
        IWebHostEnvironment environment,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _absoluteImagesDir = environment.ContentRootPath + "images";
    }

    // GET: api/Word
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Word>>> GetWords(
        [FromQuery] string? categorySlug,
        [FromQuery] bool? withDrafts,
        [FromQuery] int limit = 30,
        [FromQuery] int offset = 0)
    {
        var q = _context.Words.AsQueryable();

        q = q.OrderByDescending(n => n.Id)
            // .Include(c => c.VideoSections)
            .AsNoTracking()
            .Include(c => c.Category);


        if (categorySlug is not null)
        {
            var category = await _context.Categories
                .AnyAsync(c => c.Slug == categorySlug);

            if (!category) return NotFound();

            q = q
                .Where(c => c.Category.Slug == categorySlug)
                .Include(n => n.Category.ParentCategory);
        }

        q = q.Skip(offset)
            .Take(limit);

        if (withDrafts is null or false)
        {
            q = q.Where(w => !w.IsDraft);
        }


        if (offset >= limit && !await q.AnyAsync())
        {
            return NotFound();
        }

        return await q.ToListAsync();
    }


    // GET: api/Words/slug-name
    [HttpGet("{slug}")]
    public async Task<ActionResult<GetWordDto>> GetWordBySlug(
        string slug,
        [FromQuery] int commentsLimit = 50,
        [FromQuery] int commentsOffset = 0)
    {
        // todo need optimise queries 
        var wordQueryable = _context.Words
            .Include(c => c.Tags)

            // first time to include user and only parent comments
            .Include(m => m.Comments
                .OrderByDescending(c => c.Likes)
                .ThenBy(c => c.Dislikes)
                .ThenByDescending(c => c.Id)
                .Skip(commentsOffset)
                .Take(commentsLimit)
                .Where(c => c.ParentCommentId == null && c.Status == CommentStatus.Active)
            )
            .ThenInclude(c => c.User)

            // second time for include child comments
            .Include(m => m.Comments
                .OrderByDescending(c => c.Likes)
                .ThenBy(c => c.Dislikes)
                .ThenByDescending(c => c.Id)
                .Skip(commentsOffset)
                .Take(commentsLimit)
                .Where(c => c.ParentCommentId == null && c.Status == CommentStatus.Active)
            )
            .ThenInclude(c => c.ChildComments
                .OrderByDescending(mc => mc.Likes)
                .ThenBy(mc => mc.Dislikes)
                .ThenByDescending(mc => mc.Id)
                .Skip(0)
                .Take(50)
                .Where(mc => mc.ParentCommentId != null && mc.Status == CommentStatus.Active)
            )
            .Include(c => c.Category)
            .ThenInclude(cat => cat.ParentCategory)
            .AsNoTracking()
            .AsQueryable();

        if (User.Identity is { IsAuthenticated: true })
            if (Guid.TryParse(User.Identity?.Name, out var guid))
                wordQueryable = wordQueryable
                    .Include(m => m.UserWordLikes
                        .Where(g => g.User.Account.Guid == guid && g.Word.Slug == slug))
                    .Include(m => m.UserWordFavorites
                        .Where(f => f.Word.Slug == slug && f.User.Account.Guid == guid));


        var word = await wordQueryable
            .SingleOrDefaultAsync(m => m.Slug == slug);

        if (word == null || word.IsDraft) return NotFound();


        // todo concatenate queries with index and nearby
        var qWords = _context.Words
            .OrderBy(n => n.Name);

        var wordIndex = qWords
            .AsEnumerable()
            .Select((w, i) => w.Id == word.Id ? i : -1)
            .FirstOrDefault(i => i > -1);

        var nearbyWords = qWords
            .AsEnumerable()
            .Select(n => new Word { Id = n.Id, Slug = n.Slug, Name = n.Name })
            .Where((_, index) => index >= wordIndex - 10 && index <= wordIndex + 10)
            .ToList();

        return new GetWordDto
        {
            Word = word,
            NearbyWords = nearbyWords
        };
    }

    // GET: api/Words/GetWordById/id-number
    [HttpGet("GetWordById/{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Word>> GetWordById([FromRoute] long id)
    {
        var m = await _context.Words
            .Include(m => m.Tags)
            .SingleOrDefaultAsync(m => m.Id == id);
        if (m is null) return NotFound();

        return m;
    }

    [HttpPost("AddComment")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<AddedWordDto>> AddComment(
        [FromBody] AddCommentDto dto)
    {
        if (!Guid.TryParse(User.Identity?.Name, out var guid)) return Unauthorized();


        var account = await _context.Accounts
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Guid == guid);

        if (account is null) return NotFound();

        var comment = new WordComment
        {
            Title = dto.Title,
            Message = dto.Message,
            WordId = dto.WordId,
            ParentCommentId = dto.ParentCommentId,
            UserId = account.User.Id,
            Status = CommentStatus.Moderation
        };

        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return new AddedWordDto
        {
            CommentId = comment.Id,
            Status = comment.Status,
            Title = comment.Title,
            Message = comment.Message,
            ParentCommentId = comment.ParentCommentId
        };
    }


    // POST: api/Words/UpdateLike
    [HttpPost("UpdateLike")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<UpdatedLike>> UpdateLike(
        [FromBody] PostUpdateLikeDto body)
    {
        if (!Guid.TryParse(User.Identity?.Name, out var guid)) return Unauthorized();

        var word = await _context.Words.FindAsync(body.WordId);
        if (word is null) return NotFound();


        var user = await _context.Users
            .SingleOrDefaultAsync(u => u.Account.Guid == guid);
        if (user is null) return NotFound();


        var like = await _context.UserWordsLikes
            .FindAsync(word.Id, user.Id);


        if (like is not null)
            switch (like.LikeType)
            {
                case LikeType.Dislike:
                    word.DislikesCount -= 1;
                    break;
                case LikeType.Like:
                    word.LikesCount -= 1;
                    break;
            }

        if (like is not null && like.LikeType == body.Type)
        {
            word.LikesRate = word.LikesCount - word.DislikesCount;
            _context.UserWordsLikes.Remove(like);

            await _context.SaveChangesAsync();
            return new UpdatedLike
            {
                DislikesCount = word.DislikesCount,
                LikesCount = word.LikesCount,
                Rate = word.LikesRate,
                RemoveVote = true
            };
        }


        if (like is not null)
        {
            like.LikeType = body.Type;
        }
        else
        {
            like = new UserWordLike
            {
                UserId = user.Id,
                WordId = word.Id,
                LikeType = body.Type
            };
            await _context.UserWordsLikes.AddAsync(like);
        }

        switch (body.Type)
        {
            case LikeType.Dislike:
                word.DislikesCount += 1;
                break;
            case LikeType.Like:
                word.LikesCount += 1;
                break;
        }

        word.LikesRate = word.LikesCount - word.DislikesCount;


        await _context.SaveChangesAsync();


        return new UpdatedLike
        {
            DislikesCount = word.DislikesCount,
            LikesCount = word.LikesCount,
            Rate = word.LikesRate,
            RemoveVote = false
        };
    }

    // PUT: api/Words/5
    // To protect from over posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Word>> PutWord(
        long id, UpdateWordDto dto
    )
    {
        if (id != dto.Id) return BadRequest();

        // todo implement update image file

        var m = await _context.Words
            .Include(m => m.Tags)
            .SingleOrDefaultAsync(m => m.Id == id);

        if (m is null) return NotFound();


        if (m.Slug != dto.Slug && m.MainImage != null && !dto.RemoveImage)
        {
            var fileInfo = new FileInfo($"{_absoluteImagesDir}/{m.MainImage}");
            fileInfo.Directory?.MoveTo($"{_absoluteImagesDir}/{ImagesSubDir}/{dto.Slug}");

            m.MainImage = $"{ImagesSubDir}/{dto.Slug}/main-image.jpeg";
            m.MainThumb = $"{ImagesSubDir}/{dto.Slug}/main-thumb.jpeg";
        }

        if (dto.RemoveImage)
        {
            if (Directory.Exists($"{_absoluteImagesDir}/{ImagesSubDir}/{m.Slug}"))
                Directory.Delete($"{_absoluteImagesDir}/{ImagesSubDir}/{m.Slug}", true);

            m.MainThumb = null;
            m.MainImage = null;
        }


        m.CategoryId = dto.CategoryId;
        m.Slug = dto.Slug;
        m.UpdatedAt = DateTime.UtcNow;
        m.Name = dto.Name;
        m.Description = dto.Description;
        m.Article = dto.Article;
        m.NewsLink = dto.NewsLink;
        m.Popular = dto.Popular;
        m.Tags = new List<Tag>();
        m.LikesCount = dto.Likes;
        m.DislikesCount = dto.DisLikes;
        m.IsDraft = dto.IsDraft;

        if (dto.WordSection is not null) m.WordSection = dto.WordSection;


        if (dto.Tags is not null)
            foreach (var t in dto.Tags)
            {
                Tag? tag;

                if (t.Id != null)
                    tag = await _context.Tags.FindAsync(t.Id);
                else
                    tag = await _context.Tags
                              .SingleOrDefaultAsync(mt => mt.Slug == t.Slug)
                          ?? new Tag
                          {
                              Name = t.Name,
                              Slug = t.Slug
                          };

                if (tag != null) m.Tags.Add(tag);
            }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            if (e.InnerException?.Data["SqlState"]?.Equals("23505") ?? false)
                return Conflict(e.InnerException?.Data["MessageText"]);

            throw;
        }


        return m;
    }

    // PUT: api/Words/5/UpdateWordMainImage
    [HttpPut("{newsId:long}/UpdateWordMainImage")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Word>> UpdateWordMainImage(
        [FromRoute] long newsId,
        [FromForm] IFormFile image)
    {
        var word = await _context.Words.FindAsync(newsId);
        if (word is null) return NotFound();


        if (word.MainImage is null)
        {
            word.MainImage = $"{ImagesSubDir}/{word.Slug}/main-image.jpeg";
            word.MainThumb = $"{ImagesSubDir}/{word.Slug}/main-thumb.jpeg";
            if (!Directory.Exists($"{_absoluteImagesDir}/{ImagesSubDir}/{word.Slug})"))
                Directory.CreateDirectory($"{_absoluteImagesDir}/{ImagesSubDir}/{word.Slug}");
        }

        await _context.SaveChangesAsync();

        // save image to dir
        await using (var s = new FileStream($"{_absoluteImagesDir}/{word.MainImage}", FileMode.Create))
        {
            await image.CopyToAsync(s);
        }

        // gen thumb
        using (var thumb = await Image.LoadAsync($"{_absoluteImagesDir}/{word.MainImage}"))
        {
            thumb.Mutate(c => c.Resize(new ResizeOptions
            {
                Size = new Size(300, 300),
                Mode = ResizeMode.Max
            }));
            await thumb.SaveAsJpegAsync($"{_absoluteImagesDir}/{word.MainThumb}");
        }


        return word;
    }

    [HttpGet("AddWordDtoInitial")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AddWordDtoInitial>> AddWordDtoInitial()
    {
        return new AddWordDtoInitial
        {
            Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .Include(c => c.ParentCategory)
                .ToListAsync()
        };
    }

    // POST: api/Words
    // To protect from over posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Word>> PostWord(
        [FromForm] IFormFile? image,
        [FromForm] string jsonDto
    )
    {
        var dto = JsonSerializer.Deserialize<AddWordDto>(jsonDto);
        if (dto is null) return UnprocessableEntity();

        if (await _context.Words.AnyAsync(m => m.Slug == dto.Slug)) return Conflict();


        var word = new Word
        {
            CategoryId = dto.CategoryId,
            Name = dto.Name,
            Slug = dto.Slug,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            MainThumb = null,
            Description = dto.Description,
            Article = dto.Article,
            NewsLink = dto.NewsLink,
            Tags = new List<Tag>(),
            IsFree = false,
            Popular = dto.Popular,
            LikesCount = dto.Likes,
            DislikesCount = dto.DisLikes,
            IsDraft = dto.IsDraft,
        };

        if (dto.Tags is not null)
            foreach (var t in dto.Tags)
            {
                Tag? tag;

                if (t.Id != null)
                    tag = await _context.Tags.FindAsync(t.Id);
                else
                    tag = await _context.Tags
                              .SingleOrDefaultAsync(mt => mt.Slug == t.Slug)
                          ?? new Tag
                          {
                              Name = t.Name,
                              Slug = t.Slug
                          };

                if (tag != null) word.Tags.Add(tag);
            }


        // add image
        if (image is not null)
        {
            var relativeFolder = $"{ImagesSubDir}/{dto.Slug}";

            const string filename = "main-image.jpeg";
            const string thumbName = "main-thumb.jpeg";

            var absoluteFolder = $"{_absoluteImagesDir}/{relativeFolder}";

            Directory.CreateDirectory(absoluteFolder);


            var uploadPath = $"{absoluteFolder}/{filename}";

            await using (var fileStream = new FileStream(uploadPath, FileMode.CreateNew))
            {
                await image.CopyToAsync(fileStream);
            }

            word.MainImage = $"{relativeFolder}/{filename}".Replace("\\", "/");


            // gen thumb
            using (var thumb = await Image.LoadAsync(uploadPath))
            {
                thumb.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(300, 300),
                    Mode = ResizeMode.Max
                }));

                var absThumbPath = $"{absoluteFolder}/{thumbName}";

                word.MainThumb = $"{relativeFolder}/{thumbName}".Replace("\\", "/");
                await thumb.SaveAsJpegAsync(absThumbPath);
            }
        }


        await _context.Words.AddAsync(word);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            if (e.InnerException?.Data["SqlState"]?.Equals("23505") ?? false)
                return Conflict(e.InnerException?.Data["MessageText"]);

            throw;
        }

        return word;
    }

    // DELETE: api/Words/5
    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteWord(long id)
    {
        var word = await _context.Words.FindAsync(id);
        if (word == null) return NotFound();

        // remove images
        if (Directory.Exists($"{_absoluteImagesDir}/{ImagesSubDir}/{word.Slug}"))
            Directory.Delete($"{_absoluteImagesDir}/{ImagesSubDir}/{word.Slug}", true);

        _context.Words.Remove(word);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{newsId:long}/UpdateFavorites")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<object>> AddToFavorites(
        [FromRoute] long newsId,
        [FromQuery] bool remove)
    {
        if (!Guid.TryParse(User.Identity?.Name, out var guid)) return Unauthorized();

        var user = await _context.Users
            .SingleOrDefaultAsync(u => u.Account.Guid == guid);

        if (user is null) return NotFound();

        UserWordFavorite? favorite;
        FavoriteStatus status;

        if (remove)
        {
            favorite = await _context.UserWordsFavorites
                .FindAsync(user.Id, newsId);

            if (favorite is not null)
            {
                _context.UserWordsFavorites.Remove(favorite);
                status = FavoriteStatus.Removed;
            }
            else
            {
                return BadRequest();
            }
        }
        else
        {
            favorite = new UserWordFavorite
            {
                UserId = user.Id,
                WordId = newsId
            };

            await _context.UserWordsFavorites.AddAsync(favorite);
            status = FavoriteStatus.Added;
        }


        await _context.SaveChangesAsync();
        return Ok(new { Status = status });
    }

    [HttpGet("GetFavorites")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<List<Word>>> GetFavorites()
    {
        if (!Guid.TryParse(User.Identity?.Name, out var guid)) return Unauthorized();

        var user = await _context.Users
            .SingleOrDefaultAsync(u => u.Account.Guid == guid);
        if (user is null) return NotFound();

        var wordList = await _context.UserWordsFavorites
            .Where(m => m.UserId == user.Id)
            .Select(f => f.Word)
            .ToListAsync();


        return wordList;
    }


    [HttpGet("{newsId:long}/AddVideosToDb")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<object>> AddVideosToDb(
        [FromRoute] long newsId)
    {
        //folder - videos/words/{id}/

        // on video server paths
        // const string videosFolder = "videos/words";
        // var newsVideoFolder = videosFolder + '/' + newsId;

        var isExists = await _context.WordsVideoSections
            .AnyAsync(s => s.WordId == newsId);

        if (isExists) return BadRequest($"News Id: \"{newsId}\" - Videos Already In DB");

        // get folders response from video server
        var client = new HttpClient();
        var server = _configuration.GetSection("VideoServer")["Server"];
        if (server is null) return BadRequest("Server not set");

        var serverToken = _configuration.GetSection("VideoServer")["Token"];

        client.BaseAddress = new Uri(server);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", serverToken);

        var res = new ResponseVideoFoldersDto();

        try
        {
            res = await client
                .GetFromJsonAsync<ResponseVideoFoldersDto>("/get-video-map.php?newsId=" + newsId);
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.Unauthorized) return Unauthorized("Video Server Not Authorized");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Data);
            Console.WriteLine(e.Message);
            return BadRequest(e.Data);
        }

        if (res is null) return BadRequest(res?.Message);


        foreach (var folder in res.Folders)
        {
            var videos = new List<WordVideo>();

            // loop video files in section folder
            foreach (var file in folder.Files)
                videos.Add(new WordVideo
                {
                    Name = file.Name,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    SortNumber = file.SortNumber,
                    Folder = file.Folder,
                    Filename = file.Filename,
                    WordId = newsId,
                    IsFree = false,
                    VideoLength = file.VideoLength ?? string.Empty
                });

            // add section with videos
            var section = new WordVideoSection
            {
                Name = folder.Name,
                SortNumber = folder.SortNumber,
                Videos = videos,
                IsFree = false,
                WordId = newsId
            };
            await _context.WordsVideoSections.AddAsync(section);
        }

        await _context.SaveChangesAsync();
        return Ok(new { Message = "Added", Status = 200 });

        // end
    }
}
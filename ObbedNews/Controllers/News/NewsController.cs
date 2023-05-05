using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObbedNews.Data;
using ObbedNews.Dto.News;
using ObbedNews.Dto.VideoServer;
using ObbedNews.Enums.News;
using ObbedNews.Enums.News.Comments;
using ObbedNews.Models;
using ObbedNews.Models.Middle;

namespace ObbedNews.Controllers.News;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private const string ImagesSubDir = "news";

    private readonly string _absoluteImagesDir;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public NewsController(AppDbContext context, IWebHostEnvironment environment,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _absoluteImagesDir = environment.ContentRootPath + "images";
    }

    // GET: api/News
    // todo separate without empty for admin
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.News>>> GetNews(
        [FromQuery] string? categorySlug,
        [FromQuery] int limit = 30,
        [FromQuery] int offset = 0)
    {
        var q = _context.News.AsQueryable();

        q = q.OrderByDescending(n => n.Id)
            .Include(c => c.VideoSections)
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

        if (offset >= limit && !await q.AnyAsync())
        {
            return NotFound();
        }

        return await q.ToListAsync();
    }


    // GET: api/News/slug-name
    [HttpGet("{slug}")]
    public async Task<ActionResult<GetSingleNewsDto>> GetSingleNewsBySlug(
        string slug,
        [FromQuery] int commentsLimit = 50,
        [FromQuery] int commentsOffset = 0)
    {
        var newsQueryable = _context.News
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
            .Include(c => c.VideoSections.OrderBy(s => s.SortNumber))
            .ThenInclude(s => s.Videos.OrderBy(cv => cv.SortNumber))
            .AsQueryable();

        if (User.Identity is { IsAuthenticated: true })
            if (Guid.TryParse(User.Identity?.Name, out var guid))
                newsQueryable = newsQueryable
                    .Include(m => m.UserNewsLikes
                        .Where(g => g.User.Account.Guid == guid && g.News.Slug == slug))
                    .Include(m => m.UserNewsFavorites
                        .Where(f => f.News.Slug == slug && f.User.Account.Guid == guid));


        var news = await newsQueryable
            .SingleOrDefaultAsync(m => m.Slug == slug);

        if (news == null) return NotFound();

        var nearBy = await _context.News
            .OrderByDescending(n => n.Name)
            .Where(n => n.Id <= news.Id)
            .Take(10)
            .ToListAsync();

        nearBy.AddRange(await _context.News
            .OrderBy(n => n.Name)
            .Where(n => n.Id >= news.Id)
            .Take(10)
            .ToListAsync());


        nearBy = nearBy.OrderBy(n => n.Name).Distinct().ToList();

        return new GetSingleNewsDto
        {
            News = news,
            NearbyNews = nearBy
        };
    }

    // GET: api/News/GetNewsById/id-number
    [HttpGet("GetNewsById/{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Models.News>> GetSingleNewsById([FromRoute] long id)
    {
        var m = await _context.News
            .Include(m => m.Tags)
            .SingleOrDefaultAsync(m => m.Id == id);
        if (m is null) return NotFound();

        return m;
    }

    [HttpPost("AddComment")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<AddedNewsDto>> AddComment(
        [FromBody] AddCommentDto dto)
    {
        if (!Guid.TryParse(User.Identity?.Name, out var guid)) return Unauthorized();


        var account = await _context.Accounts
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Guid == guid);

        if (account is null) return NotFound();

        var comment = new Comment
        {
            Title = dto.Title,
            Message = dto.Message,
            NewsId = dto.NewsId,
            ParentCommentId = dto.ParentCommentId,
            UserId = account.User.Id,
            Status = CommentStatus.Moderation
        };

        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return new AddedNewsDto
        {
            CommentId = comment.Id,
            Status = comment.Status,
            Title = comment.Title,
            Message = comment.Message,
            ParentCommentId = comment.ParentCommentId
        };
    }


    // POST: api/News/UpdateLike
    [HttpPost("UpdateLike")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<UpdatedLike>> UpdateLike(
        [FromBody] PostUpdateLikeDto body)
    {
        if (!Guid.TryParse(User.Identity?.Name, out var guid)) return Unauthorized();

        var news = await _context.News.FindAsync(body.NewsId);
        if (news is null) return NotFound();


        var user = await _context.Users
            .SingleOrDefaultAsync(u => u.Account.Guid == guid);
        if (user is null) return NotFound();


        var like = await _context.UserNewsLikes
            .FindAsync(news.Id, user.Id);


        if (like is not null)
            switch (like.LikeType)
            {
                case LikeType.Dislike:
                    news.DislikesCount -= 1;
                    break;
                case LikeType.Like:
                    news.LikesCount -= 1;
                    break;
            }

        if (like is not null && like.LikeType == body.Type)
        {
            news.LikesRate = news.LikesCount - news.DislikesCount;
            _context.UserNewsLikes.Remove(like);

            await _context.SaveChangesAsync();
            return new UpdatedLike
            {
                DislikesCount = news.DislikesCount,
                LikesCount = news.LikesCount,
                Rate = news.LikesRate,
                RemoveVote = true
            };
        }


        if (like is not null)
        {
            like.LikeType = body.Type;
        }
        else
        {
            like = new UserNewsLike
            {
                UserId = user.Id,
                NewsId = news.Id,
                LikeType = body.Type
            };
            await _context.UserNewsLikes.AddAsync(like);
        }

        switch (body.Type)
        {
            case LikeType.Dislike:
                news.DislikesCount += 1;
                break;
            case LikeType.Like:
                news.LikesCount += 1;
                break;
        }

        news.LikesRate = news.LikesCount - news.DislikesCount;


        await _context.SaveChangesAsync();


        return new UpdatedLike
        {
            DislikesCount = news.DislikesCount,
            LikesCount = news.LikesCount,
            Rate = news.LikesRate,
            RemoveVote = false
        };
    }

    // PUT: api/News/5
    // To protect from over posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Models.News>> PutNews(
        long id, UpdateNewsDto dto
    )
    {
        if (id != dto.Id) return BadRequest();

        // todo implement update image file

        var m = await _context.News
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

    // PUT: api/News/5/UpdateNewsMainImage
    [HttpPut("{newsId:long}/UpdateNewsMainImage")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Models.News>> UpdateNewsMainImage(
        [FromRoute] long newsId,
        [FromForm] IFormFile image)
    {
        var news = await _context.News.FindAsync(newsId);
        if (news is null) return NotFound();


        if (news.MainImage is null)
        {
            news.MainImage = $"{ImagesSubDir}/{news.Slug}/main-image.jpeg";
            news.MainThumb = $"{ImagesSubDir}/{news.Slug}/main-thumb.jpeg";
            if (!Directory.Exists($"{_absoluteImagesDir}/{ImagesSubDir}/{news.Slug})"))
                Directory.CreateDirectory($"{_absoluteImagesDir}/{ImagesSubDir}/{news.Slug}");
        }

        await _context.SaveChangesAsync();

        // save image to dir
        await using (var s = new FileStream($"{_absoluteImagesDir}/{news.MainImage}", FileMode.Create))
        {
            await image.CopyToAsync(s);
        }

        // gen thumb
        using (var thumb = await Image.LoadAsync($"{_absoluteImagesDir}/{news.MainImage}"))
        {
            thumb.Mutate(c => c.Resize(new ResizeOptions
            {
                Size = new Size(300, 300),
                Mode = ResizeMode.Max
            }));
            await thumb.SaveAsJpegAsync($"{_absoluteImagesDir}/{news.MainThumb}");
        }


        return news;
    }

    [HttpGet("AddNewsDtoInitial")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AddNewsDtoInitial>> AddNews()
    {
        return new AddNewsDtoInitial
        {
            Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .Include(c => c.ParentCategory)
                .ToListAsync()
        };
    }

    // POST: api/News
    // To protect from over posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Models.News>> PostNews(
        [FromForm] IFormFile? image,
        [FromForm] string jsonDto
    )
    {
        var dto = JsonSerializer.Deserialize<AddNewsDto>(jsonDto);
        if (dto is null) return UnprocessableEntity();

        if (await _context.News.AnyAsync(m => m.Slug == dto.Slug)) return Conflict();


        var news = new Models.News
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
            DislikesCount = dto.DisLikes
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

                if (tag != null) news.Tags.Add(tag);
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

            news.MainImage = $"{relativeFolder}/{filename}".Replace("\\", "/");


            // gen thumb
            using (var thumb = await Image.LoadAsync(uploadPath))
            {
                thumb.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(300, 300),
                    Mode = ResizeMode.Max
                }));

                var absThumbPath = $"{absoluteFolder}/{thumbName}";

                news.MainThumb = $"{relativeFolder}/{thumbName}".Replace("\\", "/");
                await thumb.SaveAsJpegAsync(absThumbPath);
            }
        }


        await _context.News.AddAsync(news);

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

        return news;
    }

    // DELETE: api/News/5
    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteNews(long id)
    {
        var news = await _context.News.FindAsync(id);
        if (news == null) return NotFound();

        // remove images
        if (Directory.Exists($"{_absoluteImagesDir}/{ImagesSubDir}/{news.Slug}"))
            Directory.Delete($"{_absoluteImagesDir}/{ImagesSubDir}/{news.Slug}", true);

        _context.News.Remove(news);
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

        UserNewsFavorite? favorite;
        FavoriteStatus status;

        if (remove)
        {
            favorite = await _context.UserNewsFavorites
                .FindAsync(user.Id, newsId);

            if (favorite is not null)
            {
                _context.UserNewsFavorites.Remove(favorite);
                status = FavoriteStatus.Removed;
            }
            else
            {
                return BadRequest();
            }
        }
        else
        {
            favorite = new UserNewsFavorite
            {
                UserId = user.Id,
                NewsId = newsId
            };

            await _context.UserNewsFavorites.AddAsync(favorite);
            status = FavoriteStatus.Added;
        }


        await _context.SaveChangesAsync();
        return Ok(new { Status = status });
    }

    [HttpGet("GetFavorites")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<List<Models.News>>> GetFavorites()
    {
        if (!Guid.TryParse(User.Identity?.Name, out var guid)) return Unauthorized();

        var user = await _context.Users
            .SingleOrDefaultAsync(u => u.Account.Guid == guid);
        if (user is null) return NotFound();

        var newsList = await _context.UserNewsFavorites
            .Where(m => m.UserId == user.Id)
            .Select(f => f.News)
            .ToListAsync();


        return newsList;
    }


    [HttpGet("{newsId:long}/AddVideosToDb")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<object>> AddVideosToDb(
        [FromRoute] long newsId)
    {
        //folder - videos/news/{id}/

        // on video server paths
        // const string videosFolder = "videos/news";
        // var newsVideoFolder = videosFolder + '/' + newsId;

        var isExists = await _context.NewsVideoSections
            .AnyAsync(s => s.NewsId == newsId);

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
            var videos = new List<NewsVideo>();

            // loop video files in section folder
            foreach (var file in folder.Files)
                videos.Add(new NewsVideo
                {
                    Name = file.Name,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    SortNumber = file.SortNumber,
                    Folder = file.Folder,
                    Filename = file.Filename,
                    NewsId = newsId,
                    IsFree = false,
                    VideoLength = file.VideoLength ?? string.Empty
                });

            // add section with videos
            var section = new NewsVideoSection
            {
                Name = folder.Name,
                SortNumber = folder.SortNumber,
                Videos = videos,
                IsFree = false,
                NewsId = newsId
            };
            await _context.NewsVideoSections.AddAsync(section);
        }

        await _context.SaveChangesAsync();
        return Ok(new { Message = "Added", Status = 200 });

        // end
    }
}
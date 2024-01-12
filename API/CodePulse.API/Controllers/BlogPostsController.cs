using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;

//https://localhost:xxxx/api/blog
[ApiController]
[Route("api/[controller]")]
public class BlogPostsController : ControllerBase
{
    private readonly IBlogPostRepository blogPostRepository;
    private readonly ICategoryRepository categoryRepository;

    public BlogPostsController(IBlogPostRepository blogPostRepository,
    ICategoryRepository categoryRepository)
    {
        this.blogPostRepository = blogPostRepository;
        this.categoryRepository = categoryRepository;
    }

    // POST: https://localhost:xxxx/api/blogposts
    [HttpPost]
    public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
    {
        // Convert Dto to domain model
        var blogPost = new BlogPost
        {
            Author = request.Author,
            Content = request.Content,
            FeaturedImageUrl = request.FeaturedImageUrl,
            IsVisible = request.IsVisible,
            PublishedDate = request.PublishedDate,
            ShortDescription = request.ShortDescription,
            Title = request.Title,
            UrlHandle = request.UrlHandle,
            Categories = new List<Category>()
        };

        foreach (var categoryGuid in request.Categories)
        {
            var existingcategory = await categoryRepository.GetById(categoryGuid);
            if (existingcategory is not null)
            {
                blogPost.Categories.Add(existingcategory);
            }
        }

        blogPost = await blogPostRepository.CreateAsync(blogPost);

        // Convert Domain model to Dto
        var response = new BlogPostDto
        {
            Id = blogPost.Id,
            Author = blogPost.Author,
            Content = blogPost.Content,
            FeaturedImageUrl = blogPost.FeaturedImageUrl,
            IsVisible = blogPost.IsVisible,
            PublishedDate = blogPost.PublishedDate,
            ShortDescription = blogPost.ShortDescription,
            Title = blogPost.Title,
            UrlHandle = blogPost.UrlHandle,
            Categories = blogPost.Categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                UrlHandle = x.UrlHandle
            }).ToList()
        };

        return Ok(response);

    }

    // GET : https://localhost:xxxx/api/blogposts
    [HttpGet]
    public async Task<IActionResult> GetAllBlogPosts()
    {
        var blogPosts = await blogPostRepository.GetAllAsync();

        // convert Domain model to Dto
        var response = new List<BlogPostDto>();
        foreach (var blogPost in blogPosts)
        {
            response.Add(new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            });
        }
        return Ok(response);
    }

    // GET: https://localhost:xxxx/api/blogposts/{id}
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
    {
        //Get the Blog post from repository
        var blogPost = await blogPostRepository.GetByIdAsync(id);

        if (blogPost is null)
        {
            return NotFound();
        }

        //Convert Domain model to Dto
        var response = new BlogPostDto
        {
            Id = blogPost.Id,
            Author = blogPost.Author,
            Content = blogPost.Content,
            FeaturedImageUrl = blogPost.FeaturedImageUrl,
            IsVisible = blogPost.IsVisible,
            PublishedDate = blogPost.PublishedDate,
            ShortDescription = blogPost.ShortDescription,
            Title = blogPost.Title,
            UrlHandle = blogPost.UrlHandle,
            Categories = blogPost.Categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                UrlHandle = x.UrlHandle
            }).ToList()
        };

        return Ok(response);
    }

    // PUT: https://localhost:xxxx/api/blogposts/{id}
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request)
    {
        //convert Dto to domain model
        var blogPost = new BlogPost
        {
            Id = id,
            Author = request.Author,
            Content = request.Content,
            FeaturedImageUrl = request.FeaturedImageUrl,
            IsVisible = request.IsVisible,
            PublishedDate = request.PublishedDate,
            ShortDescription = request.ShortDescription,
            Title = request.Title,
            UrlHandle = request.UrlHandle,
            Categories = new List<Category>()
        };

        // Foreach
        foreach (var categoryGuid in request.Categories)
        {
            var existingCategory = await categoryRepository.GetById(categoryGuid);

            if (existingCategory != null)
            {
                blogPost.Categories.Add(existingCategory);
            }
        }

        // Call repository to update BlogPost domain model
        var updatedBlogPost = await blogPostRepository.UpdateAsync(blogPost);

        if (updatedBlogPost == null)
        {
            return NotFound();
        }

        //Convert Domain model to Dto
        var response = new BlogPostDto
        {
            Id = blogPost.Id,
            Author = blogPost.Author,
            Content = blogPost.Content,
            FeaturedImageUrl = blogPost.FeaturedImageUrl,
            IsVisible = blogPost.IsVisible,
            PublishedDate = blogPost.PublishedDate,
            ShortDescription = blogPost.ShortDescription,
            Title = blogPost.Title,
            UrlHandle = blogPost.UrlHandle,
            Categories = blogPost.Categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                UrlHandle = x.UrlHandle
            }).ToList()
        };

        return Ok(response);

    }

    // DELETE: https://localhost:xxxx/api/blogposts/{id}
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
    {
        var deletedBlogPost = await blogPostRepository.DeleteAsync(id);

        if (deletedBlogPost == null)
        {
            return NotFound();
        }

        // Convert Domain model to Dto
        var response = new BlogPostDto
        {
            Id = deletedBlogPost.Id,
            Author = deletedBlogPost.Author,
            Content = deletedBlogPost.Content,
            FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
            IsVisible = deletedBlogPost.IsVisible,
            PublishedDate = deletedBlogPost.PublishedDate,
            ShortDescription = deletedBlogPost.ShortDescription,
            Title = deletedBlogPost.Title,
            UrlHandle = deletedBlogPost.UrlHandle
            // Categories = deletedBlogPost.Categories.Select(x => new CategoryDto
            // {
            //     Id = x.Id,
            //     Name = x.Name,
            //     UrlHandle = x.UrlHandle
            // }).ToList()
        };

        return Ok(response);
    }
}
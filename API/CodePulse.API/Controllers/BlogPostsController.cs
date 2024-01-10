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

    public BlogPostsController(IBlogPostRepository blogPostRepository)
    {
        this.blogPostRepository = blogPostRepository;
    }

    // POST: https://localhost:xxxx/api/blogposts
    [HttpPost]
    public async Task<IActionResult> CreateBlogPost(CreateBlogPostRequestDto request)
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
            UrlHandle = request.UrlHandle
        };

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
            UrlHandle = blogPost.UrlHandle
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
                UrlHandle = blogPost.UrlHandle
            });
        }
        return Ok(response);
    }
}
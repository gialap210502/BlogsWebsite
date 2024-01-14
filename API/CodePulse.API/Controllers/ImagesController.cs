using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using CodePulse.API.Models.DTO;

namespace CodePulse.API.Controllers;

//https://localhost:xxxx/api/images
[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly IImageRepository imageRepository;

    public ImagesController(IImageRepository imageRepository)
    {
        this.imageRepository = imageRepository;
    }

    // GET: {apibaseurl}/api/images
    [HttpGet]
    public async Task<IActionResult> GetAllImages()
    {
        // call image repository to get all images
        var images = await imageRepository.GetAll();

        // convert this doamin model to Dto
        var response = new List<BlogImageDto>();

        foreach (var image in images)
        {
            response.Add(new BlogImageDto
            {
                Id = image.Id,
                Title = image.Title,
                DateCreated = image.DateCreated,
                FileExtension = image.FileExtension,
                FileName = image.FileName,
                Url = image.Url
            });
        }

        return Ok(response);
    }

    // POST: {apibaseurl}/api/images
    [HttpPost]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file,
    [FromForm] string fileName, [FromForm] string title)
    {
        ValidateFileUpload(file);

        if (ModelState.IsValid)
        {
            // File upload
            var blogImage = new BlogImage
            {
                FileExtension = Path.GetExtension(file.FileName).ToLower(),
                FileName = fileName,
                Title = title,
                DateCreated = DateTime.Now
            };

            await imageRepository.Upload(file, blogImage);

            //Convert Domain Model to Dto
            var response = new BlogImageDto
            {
                Id = blogImage.Id,
                Title = blogImage.Title,
                DateCreated = blogImage.DateCreated,
                FileExtension = blogImage.FileExtension,
                FileName = blogImage.FileName,
                Url = blogImage.Url
            };

            return Ok(response);

        }

        return BadRequest(ModelState);
    }

    private void ValidateFileUpload(IFormFile file)
    {
        var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

        if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
        {
            ModelState.AddModelError("file", "Unsupported file format");
        }

        if (file.Length > 10485760)
        {
            ModelState.AddModelError("file", "File size cannot be more than 10MB");

        }
    }
}
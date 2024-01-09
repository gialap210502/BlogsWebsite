using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CodePulse.API.Models.DTO;
using CodePulse.API.Models.Domain;
using CodePulse.API.Data;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using CodePulse.API.Repositories.Interface;

namespace CodePulse.API.Controllers;

//https://localhost:xxxx/api/categories
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        this.categoryRepository = categoryRepository;
    }


    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
    {
        // Map dto to domain model
        var category = new Category
        {
            Name = request.Name,
            UrlHandle = request.UrlHandle
        };

        await categoryRepository.CreateAsync(category);


        // Domain model to Dto
        var response = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            UrlHandle = category.UrlHandle
        };

        return Ok(response);

    }

    // GET: http://locahost:5150/api/categories
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        //domain
        var categories = await categoryRepository.GetAllAsync();

        //map domain model to dto
        var response = new List<CategoryDto>();
        foreach (var category in categories)
        {
            response.Add(new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            });
        }

        return Ok(response);
    }

    // GET: http://locahost:5150/api/categories/{id}
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
    {
        var existingCategory = await categoryRepository.GetById(id);

        if (existingCategory == null)
        {
            return NotFound();
        }

        var response = new CategoryDto
        {
            Id = existingCategory.Id,
            Name = existingCategory.Name,
            UrlHandle = existingCategory.UrlHandle
        };

        return Ok(response);
    }

    // PUT: http://locahost:5150/api/categories/{id}
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDto request)
    {
        // Convert Dto to domain model
        var category = new Category
        {
            Id = id,
            Name = request.Name,
            UrlHandle = request.UrlHandle
        };

        category = await categoryRepository.UpdateAsync(category);

        if (category == null)
        {
            return NotFound();
        }

        //Convert Domain to Dto
        var response = new Category
        {
            Id = category.Id,
            Name = category.Name,
            UrlHandle = category.UrlHandle
        };

        return Ok(response);
    }

    // DELETE http://locahost:5150/api/categories/{id}
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
    {
        var category = await categoryRepository.DeleteAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        // Convert domain model to Dto
        var response = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            UrlHandle = category.UrlHandle
        };

        return Ok(response);    
    }
}
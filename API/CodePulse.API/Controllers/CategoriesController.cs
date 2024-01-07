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
[Route("[controller]")]
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
}
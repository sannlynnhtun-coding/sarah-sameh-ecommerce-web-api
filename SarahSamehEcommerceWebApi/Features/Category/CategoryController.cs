using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SarahSamehEcommerceWebApi.Models.Common;

namespace SarahSamehEcommerceWebApi.Features.Category;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        this._categoryRepository = categoryRepository;
    }

    [HttpGet]
    [Authorize]
    public ActionResult<GeneralResponse> GetAllCategory()
    {
        List<Models.Category> categories = _categoryRepository.GetAll();
        List<CategoryWithProduct> categoriesWithProduct = categories.Select(category =>
            new CategoryWithProduct
            {
                Id = category.Id,
                CategoryName = category.Name,
                ProductNames = category.Products?.Select(p => p.Name).ToList()
            }).ToList();
        //return Ok(categoriesWithProduct);
        GeneralResponse response = new GeneralResponse()
        {
            IsPass = true,
            Message = categoriesWithProduct
        };
        return response;
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public ActionResult<GeneralResponse> GetById(int id)
    {
        var category = _categoryRepository.GetById(id);

        if (category == null)
        {
            // return NotFound();
            GeneralResponse localResponse = new GeneralResponse()
            {
                IsPass = false,
                Message = "Not Found"
            };
            return localResponse;
        }

        var categoryWithProducts = new CategoryWithProduct
        {
            Id = category.Id,
            CategoryName = category.Name,
            ProductNames = category.Products.Select(p => p.Name).ToList()
        };


        // return Ok(categoryWithProducts);
        GeneralResponse response = new GeneralResponse()
        {
            IsPass = true,
            Message = categoryWithProducts
        };
        return response;
    }

    [HttpPost]
    [Authorize]
    public ActionResult<GeneralResponse> AppCategory(CatDto catDto)
    {
        if (ModelState.IsValid == true)
        {
            var category = new Models.Category
            {
                Name = catDto.Name
                //  products = catDTO.ProductNames.Select(name => new Product { Name = name }).ToList()
            };
            _categoryRepository.Insert(category);
            _categoryRepository.Save();
            // return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
            return new GeneralResponse
            {
                IsPass = true,
                Message = new
                {
                    category.Id,
                    category.Name,

                }
            };
        }
        else
        {
            GeneralResponse localresponse = new GeneralResponse()
            {
                IsPass = false,
                Message = "Cant Add Category"
            };
            return localresponse;
        }


    }

    [HttpPut]
    [Authorize]
    public ActionResult<GeneralResponse> Edit(int id, CatDto updatedCategory)
    {
        Models.Category oldCategory = _categoryRepository.GetById(id);
        if (oldCategory == null)
        {
            // return NotFound();
            GeneralResponse localResponse = new GeneralResponse()
            {
                IsPass = false,
                Message = "Not Found"
            };
            return localResponse;
        }
        oldCategory.Name = updatedCategory.Name;
        _categoryRepository.Update(oldCategory);
        _categoryRepository.Save();

        GeneralResponse response = new GeneralResponse()
        {
            IsPass = true,
            Message = new
            {
                oldCategory.Id,
                oldCategory.Name,

            }
        };
        return response;

    }


    [HttpDelete("{id:int}")]
    [Authorize]
    public IActionResult Remove(int id)
    {
        try
        {
            _categoryRepository.Delete(id);
            _categoryRepository.Save();
            GeneralResponse localresponse = new GeneralResponse()
            {
                IsPass = true,
                Message = "Done"
            };
            return Ok(localresponse);
        }
        catch (Exception ex)
        {
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = false,
                Message = ex.Message

            };
            return StatusCode(500, response);
        }
    }

}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;


        public ProductController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<GeneralResponse> GetAll()
        {
            List<Product> products = _productRepository.GetAll();
            List<ProductDto> productDtOs = products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,

            }).ToList();
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = productDtOs
            };
            return response;


        }

        [HttpGet("{id:int}")]
        [Authorize]
        public ActionResult<GeneralResponse> GetById(int id)
        {
            Product product = _productRepository.GetById(id);

            if (product == null)
            {
                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Id Not Found"
                };
                return localresponse;
            }
            var productDTo = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId

            };

            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = productDTo
            };
            return response;


        }



        [HttpPost]
        [Authorize]
        public ActionResult<GeneralResponse> AddProduct(ProductDto newProductDto)
        {
            if (ModelState.IsValid == true)
            {
                var newProduct = new Product
                {
                    Name = newProductDto.Name,
                    Price = newProductDto.Price,
                    Description = newProductDto.Description,
                    CategoryId = newProductDto.CategoryId,
                };
                _productRepository.Insert(newProduct);
                _productRepository.Save();


                return new GeneralResponse
                {
                    IsPass = true,
                    Message = new
                    {
                        newProduct.Id,
                        newProduct.Name,
                        newProduct.Price,
                        newProduct.Description
                    }
                };

            }
            GeneralResponse localresponse = new GeneralResponse()
            {
                IsPass = false,
                Message = "Cant Add"
            };
            return localresponse;
        }




        [HttpPut]
        [Authorize]
        public ActionResult<GeneralResponse> Edit(int id, ProductDto updatedProduct)
        {
            Product oldproduct = _productRepository.GetById(id);

            if (oldproduct == null)
            {

                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Id Not Found"
                };
                return localresponse;
            }

            oldproduct.Name = updatedProduct.Name;
            oldproduct.Price = updatedProduct.Price;
            oldproduct.Description = updatedProduct.Description;
            oldproduct.CategoryId = updatedProduct.CategoryId;
            _productRepository.Update(oldproduct);
            _productRepository.Save();
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = new
                {
                    oldproduct.Id,
                    oldproduct.Name,
                    oldproduct.Price,
                    oldproduct.Description
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
                _productRepository.Delete(id);
                _productRepository.Save();
                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = true,
                    Message = "done"
                };
                return Ok(localresponse);
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);

                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = ex.Message

                };
                return StatusCode(500, localresponse);
            }
        }


    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SarahSamehEcommerceWebApi.Models;
using SarahSamehEcommerceWebApi.Models.Common;

namespace SarahSamehEcommerceWebApi.Features.Cart;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICartRepository _cartRepository;

    public CartController(UserManager<ApplicationUser> userManager, ICartRepository cartRepository)
    {
        this._userManager = userManager;
        this._cartRepository = cartRepository;
    }

    [HttpGet]
    [Authorize]
    public ActionResult<GeneralResponse> GetAll()
    {
        var cartsWithProductNames = _cartRepository.GetAll()
            .Select(cart => new
            {
                cart.Id,
                cart.Quantity,
                Product_Id = cart.ProductId,
                CustomerName = cart.Customer.UserName,
                CustomerEmail = cart.Customer.Email,
                ProductNames = cart.Product.Name
            })
            .ToList();


        GeneralResponse response = new GeneralResponse()
        {
            IsPass = true,
            Message = cartsWithProductNames
        };
        return response;

    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<GeneralResponse>> AddToCart(CartDto cartDto)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return Unauthorized("User not authenticated.");
        }
        if (ModelState.IsValid)
        {
            //var currentUser = await userManager.GetUserAsync(User);
            var cart = new Models.Cart
            {
                Quantity = cartDto.Quantity,
                ProductId = cartDto.ProductId,
                CustomerId = currentUser.Id,

            };
            _cartRepository.Insert(cart);
            _cartRepository.Save();
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = "Done"
            };
            return response;
        }
        else
        {
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = false,
                Message = "Cant Add To Cart"
            };
            return response;
        }
    }
    [HttpPut]
    [Authorize]
    public ActionResult<GeneralResponse> Edit(int id, CartDto updatedCart)
    {
        Models.Cart oldCart = _cartRepository.GetById(id);
        if (oldCart == null)
        {
            return NotFound();
        }
        oldCart.Quantity = updatedCart.Quantity;
        oldCart.ProductId = updatedCart.ProductId;




        _cartRepository.Update(oldCart);
        _cartRepository.Save();
        GeneralResponse response = new GeneralResponse()
        {
            IsPass = true,
            Message = "Done"
        };
        return response;
    }



    [HttpDelete("{id}")]
    [Authorize]
    public ActionResult<GeneralResponse> RemoveFromCart(int id)
    {
        try
        {
            _cartRepository.Delete(id);
            _cartRepository.Save();
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
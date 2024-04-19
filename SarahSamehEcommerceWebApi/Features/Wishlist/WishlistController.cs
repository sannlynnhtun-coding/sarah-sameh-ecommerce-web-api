using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SarahSamehEcommerceWebApi.Models;
using SarahSamehEcommerceWebApi.Models.Common;

namespace SarahSamehEcommerceWebApi.Features.Wishlist;

[Route("api/[controller]")]
[ApiController]
public class WishlistController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWishlistRepository _wishlistRepository;

    public WishlistController(UserManager<ApplicationUser> userManager, IWishlistRepository wishlistRepository)
    {
        this._userManager = userManager;
        this._wishlistRepository = wishlistRepository;
    }


    [HttpGet]
    [Authorize]
    public ActionResult<GeneralResponse> GetAll()
    {

        var wishListWithUserNames = _wishlistRepository.GetAll()

            .Select(wishList => new
            {
                wishList.Id,
                Product_Id = wishList.ProductId,
                wishList.Product.Name,
                wishList.Product.Price,
                wishList.Product.Description,

                CustomerName = wishList.Customer.UserName,
                CustomerEmail = wishList.Customer.Email
            })
            .ToList();

        //return Ok(wishListWithUserNames);
        GeneralResponse response = new GeneralResponse()
        {
            IsPass = true,
            Message = wishListWithUserNames
        };
        return response;
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public ActionResult<GeneralResponse> GetById(int id)
    {
        var wishList = _wishlistRepository.GetById(id);

        if (wishList == null)
        {
            return NotFound();
        }


        var wishListWithProducts = new
        {
            wishList.Id,
            Product_Id = wishList.ProductId,
            wishList.Product.Name,
            wishList.Product.Price,
            wishList.Product.Description,
            CustomerName = wishList.Customer.UserName,
            CustomerEmail = wishList.Customer.Email
        };


        //  return Ok(WishListWithProducts);
        GeneralResponse response = new GeneralResponse()
        {
            IsPass = true,
            Message = wishListWithProducts
        };
        return response;
    }


    [HttpGet("{username}")]
    [Authorize]
    public ActionResult<GeneralResponse> GetAllByUserName(string username)
    {
        var user = _userManager.FindByNameAsync(username).Result;
        if (user == null)
        {
            return NotFound($"User '{username}' not found.");
        }
        var wishLists = _wishlistRepository.GetAll()
            .Where(wishList => wishList.Customer.UserName == username)
            .Select(wishList => new WishlistDtOs
            {
                CustomerId = wishList.CustomerId,
                ProductName = wishList.Product.Name,
                ProductId = wishList.Product.Id,
                Price = wishList.Product.Price
            })
            .ToList();

        // return Ok(wishListWithUserNames);
        GeneralResponse response = new GeneralResponse()
        {
            IsPass = true,
            Message = wishLists
        };
        return response;
    }


    [HttpPost]
    [Authorize]
    public async Task<ActionResult<GeneralResponse>> AddToWishList(WishlistDto wishlistDto)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return Unauthorized("User not authenticated.");
        }
        if (ModelState.IsValid)
        {
            //var currentUser = await userManager.GetUserAsync(User);
            var wishList = new Models.Wishlist
            {
                Id = wishlistDto.Id,
                CustomerId = currentUser.Id,

                ProductId = wishlistDto.ProductId,


            };
            _wishlistRepository.Insert(wishList);
            _wishlistRepository.Save();
            //  return Ok();
            return new GeneralResponse
            {
                IsPass = true,
                Message = "Done"

            };
        }
        else
        {
            // return BadRequest();
            GeneralResponse localresponse = new GeneralResponse()
            {
                IsPass = false,
                Message = "Cant Add WishList"
            };
            return localresponse;
        }
    }

    [HttpPut]
    [Authorize]
    public ActionResult<GeneralResponse> Edit(int id, WishlistDto updatedWishlist)
    {
        Models.Wishlist oldWishlist = _wishlistRepository.GetById(id);
        if (oldWishlist == null)
        {
            //  return NotFound();
            GeneralResponse localResponse = new GeneralResponse()
            {
                IsPass = false,
                Message = "Not Found"
            };
            return localResponse;
        }
        oldWishlist.ProductId = updatedWishlist.ProductId;





        _wishlistRepository.Update(oldWishlist);
        _wishlistRepository.Save();
        //return NoContent();
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
            _wishlistRepository.Delete(id);
            _wishlistRepository.Save();
            //return NoContent();
            GeneralResponse localresponse = new GeneralResponse()
            {
                IsPass = true,
                Message = "Done"
            };
            return Ok(localresponse);
        }
        catch (Exception ex)
        {
            //return BadRequest(ex.Message);
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = false,
                Message = ex.Message

            };
            return StatusCode(500, response);
        }
    }
}
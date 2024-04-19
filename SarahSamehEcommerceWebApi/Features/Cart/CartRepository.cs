using Microsoft.EntityFrameworkCore;
using SarahSamehEcommerceWebApi.Models;

namespace SarahSamehEcommerceWebApi.Features.Cart;

public class CartRepository : ICartRepository
{
    Context _context;

    // inject Context
    public CartRepository(Context context)//ask context not create 
    {
        this._context = context;
    }
    public List<Models.Cart> GetAll()
    {
        return _context.Carts
            .Include(c => c.Product)
            .Include(c => c.Customer)
            .Where(c => !c.IsDeleted)
            .ToList();
    }

    public Models.Cart GetById(int id)
    {
        return _context.Carts
            .Include(c => c.Product)
            .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
    }

    public List<Models.Cart> GetCartItemsOfCustomer(string customerId)
    {

        return GetAll().Where(items => items.CustomerId == customerId).ToList();
    }
    public int GetTotalPrice(string customerId)
    {
        int totalPrice = 0;
        foreach (Models.Cart cartItem in GetCartItemsOfCustomer(customerId))
        {
            totalPrice += (cartItem.Product.Price * cartItem.Quantity);
        }

        return totalPrice;
    }

    public void Insert(Models.Cart obj)
    {
        _context.Add(obj);
    }
    public void Update(Models.Cart obj)
    {
        _context.Update(obj);
    }

    public void Delete(int id)
    {
        Models.Cart crs = GetById(id);
        crs.IsDeleted = true;
        Update(crs);


    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
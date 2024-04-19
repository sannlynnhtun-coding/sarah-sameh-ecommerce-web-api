using Microsoft.EntityFrameworkCore;
using SarahSamehEcommerceWebApi.Models;

namespace SarahSamehEcommerceWebApi.Features.Wishlist;

public class WishlistRepository : IWishlistRepository
{
    Context _context;

    // inject Context
    public WishlistRepository(Context context)//ask context not create 
    {
        this._context = context;
    }
    public List<Models.Wishlist> GetAll()
    {
        return _context.WishLists
            .Include(p => p.Customer)
            .Include(p => p.Product)
            .ToList();
    }

    public Models.Wishlist GetById(int id)
    {
        return _context.WishLists
            .Include(p => p.Product)
            .Include(p => p.Customer)
            .FirstOrDefault(p => p.Id == id);
    }
    public void Insert(Models.Wishlist obj)
    {
        _context.Add(obj);
    }
    public void Update(Models.Wishlist obj)
    {
        _context.Update(obj);
    }

    public void Delete(int id)
    {
        Models.Wishlist crs = GetById(id);
        crs.IsDeleted = true;
        Update(crs);

    }

    public void Save()
    {
        _context.SaveChanges();
    }
    public List<Models.Wishlist> GetAllbyCustomerId(string id)
    {
        List<Models.Wishlist> wishLists = _context.WishLists.
            Where(item => item.CustomerId == id && item.IsDeleted == false).ToList();
        return wishLists;
    }

}
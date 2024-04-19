using Microsoft.EntityFrameworkCore;
using SarahSamehEcommerceWebApi.Models;

namespace SarahSamehEcommerceWebApi.Features.Product;

public class ProductRepository : IProductRepository
{
    Context _context;

    // inject Context
    public ProductRepository(Context context)//ask context not create 
    {
        this._context = context;
    }
    public List<Models.Product> GetAll()
    {
        return _context.Products
            .Include(p => p.Category)
            .ToList();
    }

    public Models.Product GetById(int id)
    {
        return _context.Products
            .Include(p => p.Category)
            .FirstOrDefault(p => p.Id == id);
    }
    public void Insert(Models.Product obj)
    {
        _context.Add(obj);
    }
    public void Update(Models.Product obj)
    {
        _context.Update(obj);
    }

    public void Delete(int id)
    {
        Models.Product crs = GetById(id);
        _context.Remove(crs);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
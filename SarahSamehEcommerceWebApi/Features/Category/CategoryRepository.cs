using Microsoft.EntityFrameworkCore;
using SarahSamehEcommerceWebApi.Models;

namespace SarahSamehEcommerceWebApi.Features.Category;

public class CategoryRepository : ICategoryRepository
{
    Context _context;

    // inject Context
    public CategoryRepository(Context context)//ask context not create 
    {
        this._context = context;
    }
    public List<Models.Category> GetAll()
    {
        return _context.Categories.Include(c => c.Products).ToList();
    }

    public Models.Category GetById(int id)
    {
        return _context.Categories
            .Include(c => c.Products)
            .FirstOrDefault(p => p.Id == id);
    }
    public void Insert(Models.Category obj)
    {
        _context.Add(obj);
    }
    public void Update(Models.Category obj)
    {
        _context.Update(obj);
    }

    public void Delete(int id)
    {
        Models.Category crs = GetById(id);
        _context.Remove(crs);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
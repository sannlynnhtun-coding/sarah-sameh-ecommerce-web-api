using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        Context _context;

        // inject Context
        public CategoryRepository(Context context)//ask context not create 
        {
            this._context = context;
        }
        public List<Category> GetAll()
        {
            return _context.Categories.Include(c => c.Products).ToList();
        }

        public Category GetById(int id)
        {
            return _context.Categories
                 .Include(c => c.Products)
                .FirstOrDefault(p => p.Id == id);
        }
        public void Insert(Category obj)
        {
            _context.Add(obj);
        }
        public void Update(Category obj)
        {
            _context.Update(obj);
        }

        public void Delete(int id)
        {
            Category crs = GetById(id);
            _context.Remove(crs);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

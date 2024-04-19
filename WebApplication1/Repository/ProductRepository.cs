using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class ProductRepository : IProductRepository
    {
        Context _context;

        // inject Context
        public ProductRepository(Context context)//ask context not create 
        {
            this._context = context;
        }
        public List<Product> GetAll()
        {
            return _context.Products
                .Include(p => p.Category)
                .ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);
        }
        public void Insert(Product obj)
        {
            _context.Add(obj);
        }
        public void Update(Product obj)
        {
            _context.Update(obj);
        }

        public void Delete(int id)
        {
            Product crs = GetById(id);
            _context.Remove(crs);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

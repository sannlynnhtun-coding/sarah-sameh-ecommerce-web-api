using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class WishListRepository : IWishListRepository
    {
        Context _context;

        // inject Context
        public WishListRepository(Context context)//ask context not create 
        {
            this._context = context;
        }
        public List<WishList> GetAll()
        {
            return _context.WishLists
                .Include(p => p.Customer)
                .Include(p => p.Product)
                .ToList();
        }

        public WishList GetById(int id)
        {
            return _context.WishLists
                .Include(p => p.Product)
                .Include(p => p.Customer)
                .FirstOrDefault(p => p.Id == id);
        }
        public void Insert(WishList obj)
        {
            _context.Add(obj);
        }
        public void Update(WishList obj)
        {
            _context.Update(obj);
        }

        public void Delete(int id)
        {
            WishList crs = GetById(id);
            crs.IsDeleted = true;
            Update(crs);

        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public List<WishList> GetAllbyCustomerId(string id)
        {
            List<WishList> wishLists = _context.WishLists.
                Where(item => item.CustomerId == id && item.IsDeleted == false).ToList();
            return wishLists;
        }

    }
}

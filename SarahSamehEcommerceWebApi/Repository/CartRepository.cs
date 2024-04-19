using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class CartRepository : ICartRepository
    {
        Context _context;

        // inject Context
        public CartRepository(Context context)//ask context not create 
        {
            this._context = context;
        }
        public List<Cart> GetAll()
        {
            return _context.Carts
                 .Include(c => c.Product)
                 .Include(c => c.Customer)
                 .Where(c => !c.IsDeleted)
                 .ToList();
        }

        public Cart GetById(int id)
        {
            return _context.Carts
                 .Include(c => c.Product)
                .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
        }

        public List<Cart> GetCartItemsOfCustomer(string customerId)
        {

            return GetAll().Where(items => items.CustomerId == customerId).ToList();
        }
        public int GetTotalPrice(string customerId)
        {
            int totalPrice = 0;
            foreach (Cart cartItem in GetCartItemsOfCustomer(customerId))
            {
                totalPrice += (cartItem.Product.Price * cartItem.Quantity);
            }

            return totalPrice;
        }

        public void Insert(Cart obj)
        {
            _context.Add(obj);
        }
        public void Update(Cart obj)
        {
            _context.Update(obj);
        }

        public void Delete(int id)
        {
            Cart crs = GetById(id);
            crs.IsDeleted = true;
            Update(crs);


        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

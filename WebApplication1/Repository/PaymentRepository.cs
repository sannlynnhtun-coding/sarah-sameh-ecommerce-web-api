using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        Context _context;

        // inject Context
        public PaymentRepository(Context context)//ask context not create 
        {
            this._context = context;
        }
        public List<Payment> GetAll()
        {
            return _context.Payments.Include(c => c.Customer).ToList();
        }

        public Payment GetById(int id)
        {
            return _context.Payments
                 .Include(c => c.Customer)
                .FirstOrDefault(p => p.Id == id);
        }
        public void Insert(Payment obj)
        {
            _context.Add(obj);
        }
        public void Update(Payment obj)
        {
            _context.Update(obj);
        }

        public void Delete(int id)
        {
            Payment crs = GetById(id);
            _context.Remove(crs);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

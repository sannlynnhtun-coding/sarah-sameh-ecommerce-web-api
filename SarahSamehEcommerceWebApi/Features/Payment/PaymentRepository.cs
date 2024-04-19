using Microsoft.EntityFrameworkCore;
using SarahSamehEcommerceWebApi.Models;

namespace SarahSamehEcommerceWebApi.Features.Payment;

public class PaymentRepository : IPaymentRepository
{
    Context _context;

    // inject Context
    public PaymentRepository(Context context)//ask context not create 
    {
        this._context = context;
    }
    public List<Models.Payment> GetAll()
    {
        return _context.Payments.Include(c => c.Customer).ToList();
    }

    public Models.Payment GetById(int id)
    {
        return _context.Payments
            .Include(c => c.Customer)
            .FirstOrDefault(p => p.Id == id);
    }
    public void Insert(Models.Payment obj)
    {
        _context.Add(obj);
    }
    public void Update(Models.Payment obj)
    {
        _context.Update(obj);
    }

    public void Delete(int id)
    {
        Models.Payment crs = GetById(id);
        _context.Remove(crs);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
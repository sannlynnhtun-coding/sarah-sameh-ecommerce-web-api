using Microsoft.EntityFrameworkCore;
using SarahSamehEcommerceWebApi.Models;

namespace SarahSamehEcommerceWebApi.Features.Shipment;

public class ShipmentRepository : IShipmentRepository
{
    Context _context;

    // inject Context
    public ShipmentRepository(Context context)
    {
        this._context = context;
    }
    public List<Models.Shipment> GetAll()
    {
        return _context.Shipments.Include(s => s.Customer).ToList();
    }

    public Models.Shipment GetById(int id)
    {
        return _context.Shipments

            .FirstOrDefault(p => p.Id == id);
    }
    public void Insert(Models.Shipment obj)
    {
        _context.Add(obj);
    }
    public void Update(Models.Shipment obj)
    {
        _context.Update(obj);
    }

    public void Delete(int id)
    {
        Models.Shipment crs = GetById(id);
        _context.Remove(crs);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
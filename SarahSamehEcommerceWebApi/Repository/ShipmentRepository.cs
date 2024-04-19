using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class ShipmentRepository : IShipmentRepository
    {
        Context _context;

        // inject Context
        public ShipmentRepository(Context context)
        {
            this._context = context;
        }
        public List<Shipment> GetAll()
        {
            return _context.Shipments.Include(s => s.Customer).ToList();
        }

        public Shipment GetById(int id)
        {
            return _context.Shipments

                .FirstOrDefault(p => p.Id == id);
        }
        public void Insert(Shipment obj)
        {
            _context.Add(obj);
        }
        public void Update(Shipment obj)
        {
            _context.Update(obj);
        }

        public void Delete(int id)
        {
            Shipment crs = GetById(id);
            _context.Remove(crs);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}


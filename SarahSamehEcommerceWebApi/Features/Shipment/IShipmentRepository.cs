namespace SarahSamehEcommerceWebApi.Features.Shipment;

public interface IShipmentRepository
{
    List<Models.Shipment> GetAll();

    Models.Shipment GetById(int id);

    void Insert(Models.Shipment obj);

    void Update(Models.Shipment obj);

    void Delete(int id);

    void Save();

}
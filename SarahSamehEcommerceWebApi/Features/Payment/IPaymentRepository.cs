namespace SarahSamehEcommerceWebApi.Features.Payment;

public interface IPaymentRepository
{
    List<Models.Payment> GetAll();

    Models.Payment GetById(int id);

    void Insert(Models.Payment obj);

    void Update(Models.Payment obj);

    void Delete(int id);

    void Save();
}
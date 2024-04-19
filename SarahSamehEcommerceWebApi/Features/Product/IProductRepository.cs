namespace SarahSamehEcommerceWebApi.Features.Product;

public interface IProductRepository
{
    List<Models.Product> GetAll();

    Models.Product GetById(int id);

    void Insert(Models.Product obj);

    void Update(Models.Product obj);

    void Delete(int id);

    void Save();
}
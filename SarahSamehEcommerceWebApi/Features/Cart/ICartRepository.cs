namespace SarahSamehEcommerceWebApi.Features.Cart;

public interface ICartRepository
{
    List<Models.Cart> GetAll();

    Models.Cart GetById(int id);
    public int GetTotalPrice(string customerId);

    public List<Models.Cart> GetCartItemsOfCustomer(string customerId);
    void Insert(Models.Cart obj);

    void Update(Models.Cart obj);

    void Delete(int id);

    void Save();
}
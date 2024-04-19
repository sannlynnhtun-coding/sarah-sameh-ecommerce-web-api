using SarahSamehEcommerceWebApi.Models;

namespace SarahSamehEcommerceWebApi.Features.Wishlist;

public interface IWishlistRepository
{
    List<Models.Wishlist> GetAll();

    Models.Wishlist GetById(int id);

    void Insert(Models.Wishlist obj);

    void Update(Models.Wishlist obj);

    void Delete(int id);

    void Save();
    public List<Models.Wishlist> GetAllbyCustomerId(string id);
}
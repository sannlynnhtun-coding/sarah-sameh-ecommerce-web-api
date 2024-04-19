namespace SarahSamehEcommerceWebApi.Features.Category;

public interface ICategoryRepository
{
    List<Models.Category> GetAll();

    Models.Category GetById(int id);

    void Insert(Models.Category obj);

    void Update(Models.Category obj);

    void Delete(int id);

    void Save();
}
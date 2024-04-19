namespace SarahSamehEcommerceWebApi.Features.Category;

public class CategoryWithProduct
{

    public int Id { get; set; }

    public string CategoryName { get; set; }

    public List<string> ProductNames { get; set; }


    //  public List<Product>? products { set; get; }

}
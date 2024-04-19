using System.ComponentModel.DataAnnotations.Schema;

namespace SarahSamehEcommerceWebApi.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }
    public List<Cart>? Carts { get; set; }
    public List<Wishlist>? WishLists { get; set; }

    [ForeignKey("category")]
    public int? CategoryId { get; set; }

    public Category? Category { set; get; }
}
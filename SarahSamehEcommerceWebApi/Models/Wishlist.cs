using System.ComponentModel.DataAnnotations.Schema;

namespace SarahSamehEcommerceWebApi.Models;

public class Wishlist
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    [ForeignKey("customer")]
    public string CustomerId { get; set; }
    public ApplicationUser Customer { get; set; }
    [ForeignKey("product")]
    public int? ProductId { get; set; }
    public Product? Product { get; set; }
}
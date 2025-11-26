using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class User : IdentityUser
{
    [MaxLength(200)]
    public string? FullName { get; set; }

    // Navigation properties for related entities
    public virtual ICollection<Product> Products { get; set; } = new List<Product>(); // Products sold by this user
    public virtual ShoppingCart? ShoppingCart { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
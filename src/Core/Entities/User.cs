using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MaxLength(500)]
    public string PasswordHash { get; set; }

    [MaxLength(20)]
    public string Phone { get; set; }

    // Foreign key for Role
    [Required]
    public string RoleId { get; set; }

    // Navigation property for Role
    [ForeignKey("RoleId")]
    public virtual Role Role { get; set; }

    // Navigation properties for related entities
    public virtual ICollection<Product> Products { get; set; } = new List<Product>(); // Products sold by this user
    public virtual ShoppingCart? ShoppingCart { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
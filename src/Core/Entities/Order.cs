using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string UserId { get; set; }

    // Navigation property for User
    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } // e.g., "Pending", "Processing", "Shipped", "Delivered", "Cancelled"

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    public string AddressId { get; set; }

    // Navigation property for Address
    [ForeignKey("AddressId")]
    public virtual Address Address { get; set; }

    // Navigation property for Order Items
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
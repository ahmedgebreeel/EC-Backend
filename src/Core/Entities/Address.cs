using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Address
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
    [MaxLength(300)]
    public string Street { get; set; }

    [Required]
    [MaxLength(100)]
    public string City { get; set; }

    [Required]
    [MaxLength(100)]
    public string State { get; set; }

    [MaxLength(20)]
    public string? PostalCode { get; set; }

    [Required]
    [MaxLength(100)]
    public string Country { get; set; }

    [MaxLength(50)]
    public string Type { get; set; } // e.g., "Home", "Work", "Billing", "Shipping"

    // Navigation property for Orders using this address
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
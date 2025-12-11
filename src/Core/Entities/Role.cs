using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class Role: IdentityRole
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override string Id { get; set; } = Guid.NewGuid().ToString();

    // Navigation property for Users with this role
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
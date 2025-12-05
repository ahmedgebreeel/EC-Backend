using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets for all entities
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ============================================
        // CATEGORY CONFIGURATION
        // ============================================
        modelBuilder.Entity<Category>(entity =>
        {
            // Self-referencing relationship for parent-child categories
            entity.HasOne(c => c.ParentCategory)
                .WithMany(c => c.ChildCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Index for faster queries
            entity.HasIndex(c => c.Name)
                .IsUnique();
        });

        // ============================================
        // PRODUCT CONFIGURATION
        // ============================================
        modelBuilder.Entity<Product>(entity =>
        {
            // Product-Category relationship
            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product-Seller (User) relationship
            entity.HasOne(p => p.Seller)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            //Auto include navigation properties
            entity.Navigation(p => p.Images).AutoInclude();

            // Indexes for better performance
            entity.HasIndex(p => p.Name);
            entity.HasIndex(p => p.CategoryId);
            entity.HasIndex(p => p.SellerId);
        });

        // ============================================
        // PRODUCT IMAGE CONFIGURATION
        // ============================================
        modelBuilder.Entity<ProductImage>(entity =>
        {
            // ProductImage-Product relationship
            entity.HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Delete images when product is deleted

            // Index for ordering
            entity.HasIndex(pi => new { pi.ProductId, pi.Position });
        });

        // ============================================
        // USER CONFIGURATION
        // ============================================
        modelBuilder.Entity<User>(entity =>
        {
            // Unique email constraint
            entity.HasIndex(u => u.Email)
                .IsUnique();

            // User-Role relationship
            entity.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(u => u.RoleId);
        });

        // ============================================
        // ADDRESS CONFIGURATION
        // ============================================
        modelBuilder.Entity<Address>(entity =>
        {
            // Address-User relationship
            entity.HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Delete addresses when user is deleted

            // Index
            entity.HasIndex(a => a.UserId);
        });

        // ============================================
        // SHOPPING CART CONFIGURATION
        // ============================================
        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            // ShoppingCart-User relationship (One-to-One or One-to-Many)
            entity.HasOne(sc => sc.User)
                .WithOne(u => u.ShoppingCart)
                .HasForeignKey<ShoppingCart>(sc => sc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index
            entity.HasIndex(sc => sc.UserId)
            .IsUnique();
        });

        // ============================================
        // CART ITEM CONFIGURATION
        // ============================================
        modelBuilder.Entity<CartItem>(entity =>
        {
            // CartItem-ShoppingCart relationship
            entity.HasOne(ci => ci.ShoppingCart)
                .WithMany(sc => sc.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // CartItem-Product relationship
            entity.HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Composite index for cart and product
            entity.HasIndex(ci => new { ci.CartId, ci.ProductId });
        });

        // ============================================
        // ORDER CONFIGURATION
        // ============================================
        modelBuilder.Entity<Order>(entity =>
        {
            // Order-User relationship
            entity.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order-Address relationship
            entity.HasOne(o => o.Address)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(o => o.UserId);
            entity.HasIndex(o => o.Status);
            entity.HasIndex(o => o.CreatedAt);
        });

        // ============================================
        // ORDER ITEM CONFIGURATION
        // ============================================
        modelBuilder.Entity<OrderItem>(entity =>
        {
            // OrderItem-Order relationship
            entity.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderItem-Product relationship
            entity.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index
            entity.HasIndex(oi => new { oi.OrderId, oi.ProductId });
        });

        // ============================================
        // ROLE CONFIGURATION
        // ============================================
        modelBuilder.Entity<Role>(entity =>
        {
            // Unique role name
            entity.HasIndex(r => r.Name)
                .IsUnique();
        });
    }
}
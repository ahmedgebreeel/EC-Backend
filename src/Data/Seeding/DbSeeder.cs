using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Seeding;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Ensure database is created and migrations are applied
        await context.Database.MigrateAsync();

        // Seed Roles
        await SeedRolesAsync(roleManager);

        // Seed Users
        await SeedUsersAsync(userManager);

        // Seed Categories
        await SeedCategoriesAsync(context);

        // Seed Products (with sellers)
        await SeedProductsAsync(context, userManager);

        // Save all changes
        await context.SaveChangesAsync();
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "Admin", "Customer", "Seller" };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private static async Task SeedUsersAsync(UserManager<User> userManager)
    {
        // Admin User
        if (await userManager.FindByEmailAsync("admin@ecommerce.com") == null)
        {
            var adminUser = new User
            {
                UserName = "admin@ecommerce.com",
                Email = "admin@ecommerce.com",
                FullName = "System Administrator",
                EmailConfirmed = true,
                PhoneNumber = "+1234567890",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // Seller Users
        var sellers = new[]
        {
            new { Email = "seller1@ecommerce.com", FullName = "Tech Store", Phone = "+1234567891" },
            new { Email = "seller2@ecommerce.com", FullName = "Fashion Hub", Phone = "+1234567892" },
            new { Email = "seller3@ecommerce.com", FullName = "Home Essentials", Phone = "+1234567893" }
        };

        foreach (var seller in sellers)
        {
            if (await userManager.FindByEmailAsync(seller.Email) == null)
            {
                var sellerUser = new User
                {
                    UserName = seller.Email,
                    Email = seller.Email,
                    FullName = seller.FullName,
                    EmailConfirmed = true,
                    PhoneNumber = seller.Phone,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(sellerUser, "Seller123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(sellerUser, new[] { "Seller", "Customer" });
                }
            }
        }

        // Customer Users
        var customers = new[]
        {
            new { Email = "customer1@example.com", FullName = "John Doe", Phone = "+1234567894" },
            new { Email = "customer2@example.com", FullName = "Jane Smith", Phone = "+1234567895" },
            new { Email = "customer3@example.com", FullName = "Bob Johnson", Phone = "+1234567896" }
        };

        foreach (var customer in customers)
        {
            if (await userManager.FindByEmailAsync(customer.Email) == null)
            {
                var customerUser = new User
                {
                    UserName = customer.Email,
                    Email = customer.Email,
                    FullName = customer.FullName,
                    EmailConfirmed = true,
                    PhoneNumber = customer.Phone,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(customerUser, "Customer123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customerUser, "Customer");
                }
            }
        }
    }

    private static async Task SeedCategoriesAsync(AppDbContext context)
    {
        if (await context.Categories.AnyAsync())
            return; // Categories already seeded

        var categories = new List<Category>
        {
            // Electronics
            new Category
            {
                Name = "Electronics",
                Description = "Electronic devices and accessories",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Name = "Computers & Laptops",
                Description = "Desktop computers, laptops, and accessories",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Name = "Smartphones & Tablets",
                Description = "Mobile phones and tablet devices",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            
            // Fashion
            new Category
            {
                Name = "Fashion",
                Description = "Clothing, shoes, and accessories",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Name = "Men's Clothing",
                Description = "Clothing for men",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Name = "Women's Clothing",
                Description = "Clothing for women",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            
            // Home & Garden
            new Category
            {
                Name = "Home & Garden",
                Description = "Home improvement and garden supplies",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Name = "Furniture",
                Description = "Home and office furniture",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            
            // Sports & Outdoors
            new Category
            {
                Name = "Sports & Outdoors",
                Description = "Sports equipment and outdoor gear",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            
            // Books & Media
            new Category
            {
                Name = "Books & Media",
                Description = "Books, movies, music, and games",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();

        // Set up parent-child relationships
        var electronics = await context.Categories.FirstAsync(c => c.Name == "Electronics");
        var fashion = await context.Categories.FirstAsync(c => c.Name == "Fashion");

        var computers = await context.Categories.FirstAsync(c => c.Name == "Computers & Laptops");
        var smartphones = await context.Categories.FirstAsync(c => c.Name == "Smartphones & Tablets");
        var mensClothing = await context.Categories.FirstAsync(c => c.Name == "Men's Clothing");
        var womensClothing = await context.Categories.FirstAsync(c => c.Name == "Women's Clothing");

        computers.ParentCategoryId = electronics.Id;
        smartphones.ParentCategoryId = electronics.Id;
        mensClothing.ParentCategoryId = fashion.Id;
        womensClothing.ParentCategoryId = fashion.Id;

        await context.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(AppDbContext context, UserManager<User> userManager)
    {
        if (await context.Products.AnyAsync())
            return; // Products already seeded

        var seller1 = await userManager.FindByEmailAsync("seller1@ecommerce.com");
        var seller2 = await userManager.FindByEmailAsync("seller2@ecommerce.com");
        var seller3 = await userManager.FindByEmailAsync("seller3@ecommerce.com");

        var computersCategory = await context.Categories.FirstAsync(c => c.Name == "Computers & Laptops");
        var smartphonesCategory = await context.Categories.FirstAsync(c => c.Name == "Smartphones & Tablets");
        var mensClothingCategory = await context.Categories.FirstAsync(c => c.Name == "Men's Clothing");
        var furnitureCategory = await context.Categories.FirstAsync(c => c.Name == "Furniture");
        var sportsCategory = await context.Categories.FirstAsync(c => c.Name == "Sports & Outdoors");

        var products = new List<Product>
        {
            // Electronics - Seller 1
            new Product
            {
                Name = "Dell XPS 15 Laptop",
                Description = "High-performance laptop with 15.6\" 4K display, Intel Core i7, 16GB RAM, 512GB SSD",
                Price = 1499.99m,
                Stock = 25,
                CategoryId = computersCategory.Id,
                SellerId = seller1!.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new ProductImage { ImageUrl = "https://via.placeholder.com/800x600?text=Dell+XPS+15", Position = 0, CreatedAt = DateTime.UtcNow }
                }
            },
            new Product
            {
                Name = "MacBook Pro 14\"",
                Description = "Apple M2 Pro chip, 16GB unified memory, 512GB SSD, stunning Liquid Retina XDR display",
                Price = 1999.99m,
                Stock = 15,
                CategoryId = computersCategory.Id,
                SellerId = seller1.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new ProductImage { ImageUrl = "https://via.placeholder.com/800x600?text=MacBook+Pro+14", Position = 0, CreatedAt = DateTime.UtcNow }
                }
            },
            new Product
            {
                Name = "iPhone 15 Pro",
                Description = "6.1\" Super Retina XDR display, A17 Pro chip, 256GB storage, Titanium design",
                Price = 1099.99m,
                Stock = 50,
                CategoryId = smartphonesCategory.Id,
                SellerId = seller1.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new ProductImage { ImageUrl = "https://via.placeholder.com/800x600?text=iPhone+15+Pro", Position = 0, CreatedAt = DateTime.UtcNow }
                }
            },
            new Product
            {
                Name = "Samsung Galaxy S24 Ultra",
                Description = "6.8\" Dynamic AMOLED display, Snapdragon 8 Gen 3, 512GB storage, 200MP camera",
                Price = 1299.99m,
                Stock = 40,
                CategoryId = smartphonesCategory.Id,
                SellerId = seller1.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new ProductImage { ImageUrl = "https://via.placeholder.com/800x600?text=Galaxy+S24+Ultra", Position = 0, CreatedAt = DateTime.UtcNow }
                }
            },

            // Fashion - Seller 2
            new Product
            {
                Name = "Men's Classic Suit",
                Description = "Premium wool blend suit, tailored fit, available in navy and charcoal",
                Price = 299.99m,
                Stock = 30,
                CategoryId = mensClothingCategory.Id,
                SellerId = seller2!.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new ProductImage { ImageUrl = "https://via.placeholder.com/800x600?text=Men's+Suit", Position = 0, CreatedAt = DateTime.UtcNow }
                }
            },
            new Product
            {
                Name = "Men's Casual Shirt",
                Description = "100% cotton, button-down collar, available in multiple colors",
                Price = 49.99m,
                Stock = 100,
                CategoryId = mensClothingCategory.Id,
                SellerId = seller2.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new ProductImage { ImageUrl = "https://via.placeholder.com/800x600?text=Casual+Shirt", Position = 0, CreatedAt = DateTime.UtcNow }
                }
            },
            new Product
            {
                Name = "Men's Denim Jeans",
                Description = "Classic fit denim jeans, durable and comfortable, multiple sizes",
                Price = 79.99m,
                Stock = 75,
                CategoryId = mensClothingCategory.Id,
                SellerId = seller2.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new ProductImage { ImageUrl = "https://via.placeholder.com/800x600?text=Denim+Jeans", Position = 0, CreatedAt = DateTime.UtcNow }
                }
            },

            // Home & Garden - Seller 3
            new Product
            {
                Name = "Modern Office Desk",
                Description = "Spacious desk with cable management, sturdy construction, walnut finish",
                Price = 349.99m,
                Stock = 20,
                CategoryId = furnitureCategory.Id,
                SellerId = seller3!.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new ProductImage { ImageUrl = "https://via.placeholder.com/800x600?text=Office+Desk", Position = 0, CreatedAt = DateTime.UtcNow }
                }
            },
            new Product
            {
                Name = "Ergonomic Office Chair",
                Description = "Adjustable height, lumbar support, breathable mesh back, 360° swivel",
                Price = 249.99m,
                Stock = 35,
                CategoryId = furnitureCategory.Id,
                SellerId = seller3.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new ProductImage { ImageUrl = "https://via.placeholder.com/800x600?text=Office+Chair", Position = 0, CreatedAt = DateTime.UtcNow }
                }
            },

            // Sports - Seller 1
            new Product
            {
                Name = "Professional Yoga Mat",
                Description = "Non-slip, eco-friendly TPE material, 6mm thick, includes carrying strap",
                Price = 39.99m,
                Stock = 60,
                CategoryId = sportsCategory.Id,
                SellerId = seller1.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new ProductImage { ImageUrl = "https://via.placeholder.com/800x600?text=Yoga+Mat", Position = 0, CreatedAt = DateTime.UtcNow }
                }
            },
            new Product
            {
                Name = "Adjustable Dumbbell Set",
                Description = "5-52.5 lbs per dumbbell, space-saving design, quick adjustment system",
                Price = 299.99m,
                Stock = 25,
                CategoryId = sportsCategory.Id,
                SellerId = seller1.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Images = new List<ProductImage>
                {
                    new ProductImage { ImageUrl = "https://via.placeholder.com/800x600?text=Dumbbell+Set", Position = 0, CreatedAt = DateTime.UtcNow }
                }
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}

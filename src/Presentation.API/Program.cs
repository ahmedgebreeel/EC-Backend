using System.Text.Json.Serialization;
using Business.Services;
using Core.Mapping;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//add DB context
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


//add UnitOfWork
builder.Services.AddScoped<UnitOfWork>();

//Add Services
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CartItemService>();
builder.Services.AddScoped<ShoppingCartService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();


// Register AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ProductProfile>();
    // add more profiles here if needed
});


var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        
        await Data.Seeding.DbSeeder.SeedAsync(context, userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
   {
       c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
   });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();


app.MapGet("/test-db", async (AppDbContext db, IConfiguration config) =>
{
    try
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        var canConnect = await db.Database.CanConnectAsync();

        return Results.Ok(new
        {
            success = canConnect,
            message = canConnect ? "Database connection successful!" : "Connection failed",
            connectionString
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
    }
});

app.Run();

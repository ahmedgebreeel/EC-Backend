using Business.Services;
using Core.Mapping;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

//add DB context
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ProductProfile>();
    // add more profiles here if needed
});


//Add Services
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CartItemService>();
builder.Services.AddScoped<ShoppingCartService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();

//add UnitOfWork
builder.Services.AddScoped<UnitOfWork>();

//Controllers Configuration
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


//OpenApi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "My Api v1"));
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

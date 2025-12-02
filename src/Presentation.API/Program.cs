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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
     app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "";
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

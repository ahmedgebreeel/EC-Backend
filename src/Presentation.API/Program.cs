using System.Text;
using System.Text.Json.Serialization;
using Business.Services;
using Core.Entities;
using Core.Mapping;
using Data;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


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

// Configure Identity with custom User and Role
builder.Services.AddIdentity<User, Role>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

//add UnitOfWork
builder.Services.AddScoped<UnitOfWork>();

//Add Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TokenService>();
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


builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularApp", policy =>
    {
        policy.AllowAnyOrigin() // Angular Port
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // Required for HttpOnly Cookies
    });
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

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AngularApp");

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

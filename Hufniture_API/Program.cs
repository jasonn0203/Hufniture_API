using Hufniture_API.Data;
using Hufniture_API.Repositories;
using Hufniture_API.Repositories.ColorRepository;
using Hufniture_API.Repositories.FurnitureCategoryRepository;
using Hufniture_API.Repositories.FurnitureProductRepository;
using Hufniture_API.Repositories.FurnitureTypeRepository;
using Hufniture_API.Repositories.Interfaces;
using Hufniture_API.Repositories.OrderItemRepository;
using Hufniture_API.Repositories.OrderRepository;
using Hufniture_API.Repositories.ReviewRepository;
using Hufniture_API.Repositories.UserRepository;
using Hufniture_API.Services.FurnitureCategoryService;
using Hufniture_API.Services.FurnitureProductService;
using Hufniture_API.Services.FurnitureTypeService;
using Hufniture_API.Services.OrderService;
using Hufniture_API.Services.ReviewService;
using Hufniture_API.Services.TokenService;
using Hufniture_API.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

//Add Db connectionstring
builder.Services.AddDbContext<HufnitureDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("HufnitureDb"));
});

//Config for Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true; // Yêu cầu có số
    options.Password.RequireUppercase = true; // Yêu cầu có chữ hoa
    options.Password.RequiredLength = 6; // Yêu cầu độ dài tối thiểu

})
    .AddEntityFrameworkStores<HufnitureDbContext>()
    .AddDefaultTokenProviders();


//Add authentication, authorization
//Config authen JWT tokens
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),

        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});





builder.Services.AddAuthorization();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin()
       .AllowAnyMethod()
       .AllowAnyHeader());
});




//Config DI ( Repositories, Services )
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//DI Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IFurnitureCategoryRepository, FurnitureCategoryRepository>();
builder.Services.AddScoped<IFurnitureTypeRepository, FurnitureTypeRepository>();
builder.Services.AddScoped<IFurnitureProductRepository, FurnitureProductRepository>();
builder.Services.AddScoped<IColorRepository, ColorRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();


//DI Service
builder.Services.AddTransient(typeof(IFurnitureCategoryService), typeof(FurnitureCategoryService));
builder.Services.AddTransient(typeof(IFurnitureTypeService), typeof(FurnitureTypeService));
builder.Services.AddTransient(typeof(IFurnitureProductService), typeof(FurnitureProductService));
builder.Services.AddTransient(typeof(IReviewService), typeof(ReviewService));
builder.Services.AddTransient(typeof(IOrderService), typeof(OrderService));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

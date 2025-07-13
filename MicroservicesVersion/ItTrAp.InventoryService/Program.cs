using FluentValidation;
using ItTrAp.InventoryService.Interfaces;
using ItTrAp.InventoryService.Interfaces.Persistence.Repositories;
using ItTrAp.InventoryService.Interfaces.Repositories;
using ItTrAp.InventoryService.Interfaces.Services;
using ItTrAp.InventoryService.Persistence;
using ItTrAp.InventoryService.Persistence.Repositories;
using ItTrAp.InventoryService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MongoDB.Driver;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

var appConfig = builder.Configuration.GetSection("GlobalConfig");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"));
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDbConnection");
    return new MongoClient(connectionString);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase("inventory_service");
});

builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        cfg.LicenseKey = appConfig["MediatrLicenseKey"];
    });

builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddRouting();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddConfiguredJwtBearerAuthentication(appConfig);

builder.Services.AddControllers();

builder.Services.AddTransient(typeof(Lazy<>), typeof(LazyResolver<>));

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IMovableItemService, MovableItemService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

builder.Services.AddScoped<ICategoryUniquenessChecker, EFCategoryReadRepository>();
builder.Services.AddScoped<ICategoryReadRepository, EFCategoryReadRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

builder.Services.AddScoped<IMovableItemUniquenessChecker, MongoMovableItemReadRepository>();
builder.Services.AddScoped<IMovableItemReadRepository, MongoMovableItemReadRepository>();
builder.Services.AddScoped<IMovableItemRepository, MongoMovableItemRepository>();


builder.Services.Configure<GlobalConfig>(appConfig);

var app = builder.Build();

app.Services.InitializeAppDb();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// #if !DEBUG
//     app.UseHttpsRedirection();
// #endif

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (!Directory.Exists(FileService.RootFolderPhysicalPath))
{
    Directory.CreateDirectory(FileService.RootFolderPhysicalPath);
}
app.UseStaticFiles(new StaticFileOptions
{
    RequestPath = Path.Combine("/", FileService.RootUrl, FileService.RootFolder),
    FileProvider = new PhysicalFileProvider(FileService.RootFolderPhysicalPath),
});

app.Run();

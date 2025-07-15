using FluentValidation;
using ItTrAp.InventoryService.Application.Interfaces.Services;
using ItTrAp.InventoryService.Domain.Interfaces;
using ItTrAp.InventoryService.Infrastructure.Interfaces;
using ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence.Repositories;
using ItTrAp.InventoryService.Infrastructure.Interfaces.Repositories;
using ItTrAp.InventoryService.Infrastructure.Persistence;
using ItTrAp.InventoryService.Infrastructure.Persistence.Repositories;
using ItTrAp.InventoryService.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MongoDB.Driver;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

var appConfig = builder.Configuration.GetSection("GlobalConfig");

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

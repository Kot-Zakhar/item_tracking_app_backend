using Abstractions;
using Database;
using FluentValidation;
using Infrastructure.EFPersistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using WebApi;
using WebApi.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsqlWithSnakeCase(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, HasPermissionHandler>();

builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });

builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

var appConfig = builder.Configuration.GetSection("ApplicationConfig");

builder.Services.Configure<GlobalConfig>(appConfig);

builder.Services.AddSingleton<IInfrastructureGlobalConfig>(sp => 
    sp.GetRequiredService<IOptions<GlobalConfig>>().Value);

builder.Services.AddTransient<IOptions<IInfrastructureGlobalConfig>>(sp => 
    Options.Create<IInfrastructureGlobalConfig>(
        sp.GetRequiredService<IOptions<GlobalConfig>>().Value));

builder.Services.AddDependencyInversionContainer();

builder.Services.AddRouting();


builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddConfiguredJwtBearerAuthentication(appConfig);

builder.Services.AddControllers();

var app = builder.Build();

app.Services.InitializeAppDb();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// TODO: Format exceptions

#if !DEBUG
    app.UseHttpsRedirection();
#endif

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

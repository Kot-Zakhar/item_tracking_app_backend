using Database;
using FluentValidation;
using Infrastructure.EFPersistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddDependencyInversionContainer();

builder.Services.AddRouting();

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("Manager", policy =>
//     {
//         policy.RequireAuthenticatedUser();
//         policy.RequireClaim("role", "manager");
//     });

//     options.AddPolicy("User", policy =>
//     {
//         policy.RequireAuthenticatedUser();
//         policy.RequireClaim("role", "user");
//     });
// });

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddConfiguredJwtBearerAuthentication(builder.Configuration);

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

app.Run();

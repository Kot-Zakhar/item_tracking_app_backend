using Database;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsqlWithSnakeCase(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDependencyInversionContainer();

builder.Services.AddRouting();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// TODO: Format exceptions

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();

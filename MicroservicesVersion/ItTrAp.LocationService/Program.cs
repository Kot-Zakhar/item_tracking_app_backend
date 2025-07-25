using FluentValidation;
using ItTrAp.LocationService.Infrastructure.Persistence.Interfaces;
using ItTrAp.LocationService.Infrastructure.Persistence;
using ItTrAp.LocationService.Infrastructure.Persistence.Interfaces.Repositories;
using ItTrAp.LocationService.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ItTrAp.LocationService.Infrastructure.Persistence.Repositories;
using ItTrAp.LocationService.Infrastructure.Services;
using ItTrAp.LocationService.Application.Interfaces.Services;
using Amazon.SimpleNotificationService;
using ItTrAp.LocationService.Domain.Interfaces;




#if DEBUG
using DotNetEnv;
    Env.Load();
#endif

var builder = WebApplication.CreateBuilder(args);

var appConfig = builder.Configuration.GetSection("GlobalConfig");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"));
    options.UseSnakeCaseNamingConvention();
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


builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

builder.Services.AddScoped<ILocationRepository, EFLocationRepository>();
builder.Services.AddScoped<ILocationReadRepository, EFLocationReadRepository>();
builder.Services.AddScoped<ILocationUniquenessChecker, EFLocationReadRepository>();

builder.Services.AddScoped<ILocationService, LocationService>();

builder.Services.AddScoped<IEventPublishingService, SnsEventPublishingService>();

builder.Services.Configure<GlobalConfig>(appConfig);

var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.DefaultClientConfig.ServiceURL = builder.Configuration["AWS:ServiceURL"];
awsOptions.DefaultClientConfig.UseHttp = true;

builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonSimpleNotificationService>();

var app = builder.Build();

app.Services.InitializeAppDb();

// #if !DEBUG
//     app.UseHttpsRedirection();
// #endif

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

using Amazon.SimpleNotificationService;
using FluentValidation;
using ItTrAp.UserService.Infrastructure.Interfaces.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ItTrAp.UserService.Infrastructure.Persistence;
using ItTrAp.UserService.Infrastructure.Persistence.Repositories;
using ItTrAp.UserService.Infrastructure.Interfaces.Persistence.Repositories;
using ItTrAp.UserService.Application.Interfaces;
using ItTrAp.UserService.Domain.Interfaces;
using ItTrAp.UserService.Infrastructure.Services;
using ItTrAp.UserService.Application.Interfaces.Services;
using ItTrAp.UserService.Jobs;


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

builder.Services.AddScoped<IUserReadRepository, EfUserReadRepository>();
builder.Services.AddScoped<IUserUniquenessChecker, EfUserReadRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IEventPublishingService, SnsEventPublishingService>();

builder.Services.Configure<GlobalConfig>(appConfig);

var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.DefaultClientConfig.ServiceURL = builder.Configuration["AWS:ServiceURL"];
awsOptions.DefaultClientConfig.UseHttp = true;

builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonSimpleNotificationService>();

builder.Services.AddHostedService<AdminInitializingJob>();

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

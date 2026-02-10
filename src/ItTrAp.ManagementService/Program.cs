using Amazon.SQS;
using FluentValidation;
using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Infrastructure.Interfaces;
using ItTrAp.ManagementService.Infrastructure.Interfaces.Persistence.Repositories;
using ItTrAp.ManagementService.Infrastructure.Persistence;
using ItTrAp.ManagementService.Infrastructure.Persistence.Repositories;
using ItTrAp.ManagementService.Infrastructure.Services;
using ItTrAp.ManagementService.Infrastructure.Workers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ItTrAp.ManagementService.Infrastructure.Interfaces.Services;
using ItTrAp.ManagementService.Application.Interfaces.Repositories;
using ItTrAp.ManagementService.Infrastructure.Behaviors;
using ItTrAp.ManagementService.Infrastructure.Servers;

#if DEBUG
using DotNetEnv;
    Env.Load();
#endif

var builder = WebApplication.CreateBuilder(args);

var httpPort = builder.Configuration.GetValue("HTTP_PORT", 80);
var grpcPort = builder.Configuration.GetValue("GRPC_PORT", 5000);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(httpPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });
    options.ListenAnyIP(grpcPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

var appConfig = builder.Configuration.GetSection("GlobalConfig");

builder.Services.AddGrpc();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"));
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
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
builder.Services.AddScoped<IUserRepository, EFUserRepository>();
builder.Services.AddScoped<IMovableItemRepository, EFMovableItemRepository>();
builder.Services.AddScoped<IMovableInstanceRepository, EFMovableInstanceRepository>();
builder.Services.AddScoped<ILocationRepository, EFLocationRepository>();
builder.Services.AddScoped<IMovableInstanceReadRepository, EFMovableInstanceRepository>();
builder.Services.AddScoped<IReservationReadRepository, EFReservationsReadRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMovableItemService, MovableItemService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IMovableInstanceService, MovableInstanceService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddScoped<IInboundEventService, InboundEventService>();

var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.DefaultClientConfig.ServiceURL = builder.Configuration["AWS:ServiceURL"];
awsOptions.DefaultClientConfig.UseHttp = true;

builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonSQS>();

builder.Services.AddHostedService<SqsPoolingWorker>();

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

app.MapGrpcService<GrpcServer>();

app.Run();

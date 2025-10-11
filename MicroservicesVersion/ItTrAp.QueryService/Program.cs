using Amazon.SQS;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ItTrAp.QueryService.Infrastructure.Behaviors;
using ItTrAp.QueryService.Infrastructure.Workers;
using ItTrAp.QueryService.Application.Interfaces;
using ItTrAp.QueryService.Infrastructure.Services;
using ItTrAp.QueryService.Infrastructure.Interfaces.Services;
using ItTrAp.QueryService.Infrastructure.Interfaces.Service;



#if DEBUG
using DotNetEnv;
    Env.Load();
#endif

var builder = WebApplication.CreateBuilder(args);

var appConfig = builder.Configuration.GetSection("GlobalConfig");

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

builder.Services.AddScoped<IQueryService, QueryService>();

builder.Services.AddScoped<IInboundEventService, InboundEventService>();

builder.Services.AddScoped<ILocationService, LocationGrpcService>();
builder.Services.AddScoped<IManagementService, ManagementGrpcService>();

var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.DefaultClientConfig.ServiceURL = builder.Configuration["AWS:ServiceURL"];
awsOptions.DefaultClientConfig.UseHttp = true;

builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonSQS>();

builder.Services.AddHostedService<SqsPoolingWorker>();

builder.Services.Configure<GlobalConfig>(appConfig);

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

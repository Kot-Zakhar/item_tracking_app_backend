using ItTrAp.UserService.Application.Commands;
using ItTrAp.UserService.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Options;

namespace ItTrAp.UserService.Jobs;

public class AdminInitializingJob(ILogger<AdminInitializingJob> logger, IServiceProvider serviceProvider, IOptions<GlobalConfig> config) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(60000, stoppingToken); // TODO: Waiting for localstack to finish its configuration

        using (var scope = serviceProvider.CreateScope())
        {
            var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

            var user = new CreateUserDto
            {
                Email = config.Value.AdminEmail,
                FirstName = "Admin",
                LastName = "User",
                Phone = config.Value.AdminPhone
            };

            try
            {
                await mediatr.Send(new CreateUserCommand(user));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the admin user.");
            }
        }
    }
}
using ItTrAp.UserService.Application.DTOs;
using ItTrAp.UserService.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace ItTrAp.UserService.Jobs;

public class AdminInitializingJob(ILogger<AdminInitializingJob> logger, IServiceProvider serviceProvider, IOptions<GlobalConfig> config) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(60000, stoppingToken); // TODO: Waiting for localstack to finish its configuration

        using (var scope = serviceProvider.CreateScope())
        {
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            try
            {
                await userService.CreateUserAsync(new CreateUserDto
                {
                    Email = config.Value.AdminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    Phone = config.Value.AdminPhone
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the admin user.");
            }
        }
    }
}
using FluentValidation;
using ItTrAp.UserService.Application.Commands;
using ItTrAp.UserService.Application.DTOs;
using ItTrAp.UserService.Application.Interfaces;
using ItTrAp.UserService.Test.Application.Helpers;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ItTrAp.UserService.Integration.Test;

public class CommandsAndQueriesPipelineTest
{
    private IMediator _mediator = null!;
    private Mock<IUserService> _userServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>();
        _userServiceMock
            .Setup(s => s.CreateUserAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((uint)1);
        _userServiceMock
            .Setup(s => s.UpdateUserAsync(It.IsAny<uint>(),It.IsAny<UpdateUserDto>(), It.IsAny<CancellationToken>()));
        _userServiceMock
            .Setup(s => s.DeleteUserAsync(It.IsAny<uint>(), It.IsAny<CancellationToken>()));

        var services = new ServiceCollection();

        services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.LicenseKey = "FREE_LICENSE_KEY";
        });

        services.AddLogging();

        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

        services.AddSingleton<IUserService>(_userServiceMock.Object);

        var provider = services.BuildServiceProvider();
        _mediator = provider.GetRequiredService<IMediator>();
    }

    [Test]
    public async Task CreateUserCommand_ValidCommand_ShouldReachHandlerAndReturnResult()
    {
        // Arrange
        var userDto = UserDtoHelper.CreateValidUserDto().ToCreateUserDto();
        var command = new CreateUserCommand(userDto);

        // Act
        var result = await _mediator.Send(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(1));
        _userServiceMock.Verify(
            s => s.CreateUserAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public void CreateUserCommand_InvalidCommand_ShouldThrowValidationExceptionBeforeHandler()
    {
        // Arrange
        var command = new CreateUserCommand(
            UserDtoHelper.CreateValidUserDto().WithLastName("").ToCreateUserDto());

        // Act
        var act = async () => await _mediator.Send(command, CancellationToken.None);

        // Assert
        var ex = act.ShouldThrow<Exception>()
            .WithMessage("Last name is required.");
        _userServiceMock.Verify(
            s => s.CreateUserAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

}
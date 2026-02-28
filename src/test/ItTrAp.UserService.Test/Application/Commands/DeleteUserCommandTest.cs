using ItTrAp.UserService.Application.Commands;
using ItTrAp.UserService.Application.Interfaces;

namespace ItTrAp.UserService.Test.Application.Commands;

public class DeleteUserCommandTest
{
    [Test]
    public void DeleteUserCommandValidator_ValidCommand_ShouldPassValidation()
    {
        // Arrange
        var command = new DeleteUserCommand(123);
        var validator = new DeleteUserCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void DeleteUserCommandValidator_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        var command = new DeleteUserCommand(0);
        var validator = new DeleteUserCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("User ID must be greater than 0.");
    }

    [Test]
    public async Task Handle_ValidCommand_ShouldCallService()
    {
        // Arrange
        var command = new DeleteUserCommand(123);
        var userServiceMock = new Mock<IUserService>();
        userServiceMock
            .Setup(s => s.DeleteUserAsync(It.IsAny<uint>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var handler = new DeleteUserHandler(userServiceMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        userServiceMock.Verify(s => s.DeleteUserAsync(It.IsAny<uint>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
using ItTrAp.UserService.Application.Interfaces;
using ItTrAp.UserService.Application.DTOs;
using ItTrAp.UserService.Application.Queries;

namespace ItTrAp.UserService.Test.Application.Queries;

public class GetUserByIdTest
{
    private Mock<IUserReadRepository> _usersRepositoryMock = new Mock<IUserReadRepository>();

    [Test]
    public void Validator_InvalidId_ReturnsValidationError()
    {
        // Arrange
        var query = new GetUserByIdQuery(0);
        var validator = new GetUserByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("User ID must be greater than 0.");
    }

    [Test]
    public async Task GetUserById_ValidId_ReturnsExpectedResult()
    {
        // Arrange
        var userId = 1u;
        var expectedUser = new UserDto { Id = userId, FirstName = "Test", LastName = "User", Phone = "+1234567890", Email = "test.user@example.com" };

        _usersRepositoryMock
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<UserDto?>(expectedUser));

        var query = new GetUserByIdQuery(userId);
        var handler = new GetUserByIdHandler(_usersRepositoryMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expectedUser.Id);
        result.FirstName.Should().Be(expectedUser.FirstName);
        result.LastName.Should().Be(expectedUser.LastName);
        result.Phone.Should().Be(expectedUser.Phone);
        result.Email.Should().Be(expectedUser.Email);
    }
}
using ItTrAp.UserService.Application.DTOs;
using ItTrAp.UserService.Application.Interfaces;
using ItTrAp.UserService.Application.Queries;

namespace ItTrAp.UserService.Test.Application.Queries;

public class GetUsersTest
{
    private Mock<IUserReadRepository> _usersRepositoryMock = new Mock<IUserReadRepository>();

    [Test]
    public async Task GetUsers_ReturnsExpectedResult()
    {
        // Arrange
        var expectedUsers = new List<UserDto>
        {
            new UserDto { Id = 1, FirstName = "Test1", LastName = "User1", Phone = "+1234567890", Email = "test1.user1@example.com" },
            new UserDto { Id = 2, FirstName = "Test2", LastName = "User2", Phone = "+1234567891", Email = "test2.user2@example.com" }
        };

        _usersRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<IList<UserDto>>(expectedUsers));

        var query = new GetUsersQuery();
        var handler = new GetUsersQueryHandler(_usersRepositoryMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(expectedUsers.Count);
        result.Should().BeEquivalentTo(expectedUsers);
    }
}
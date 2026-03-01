using ItTrAp.UserService.Application.DTOs;
using ItTrAp.UserService.Application.Interfaces;
using ItTrAp.UserService.Application.Queries;

namespace ItTrAp.UserService.Test.Application.Queries;

public class GetAllFilteredUsersTest
{
    private Mock<IUserReadRepository> _usersRepositoryMock = new Mock<IUserReadRepository>();

    [Test]
    public async Task GetAllFilteredUsers_NoBody_ReturnsExpectedResults()
    {
        // Arrange
        _usersRepositoryMock
            .Setup(r => r.GetAllFiltered(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new List<UserDto>()));

        var query = new GetAllFilteredUsersQuery(null, null);
        var handler = new GetAllFilteredUsersHandler(_usersRepositoryMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }

    [Test]
    public async Task GetAllFilteredUsers_WithSearchAndTop_ReturnsExpectedResults()
    {
        // Arrange
        _usersRepositoryMock
            .Setup(r => r.GetAllFiltered(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new List<UserDto>()));

        var query = new GetAllFilteredUsersQuery("search", 10);
        var handler = new GetAllFilteredUsersHandler(_usersRepositoryMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }
}
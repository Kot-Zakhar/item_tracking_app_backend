using ItTrAp.Shared.Test;
using ItTrAp.UserService.Application.Commands;
using ItTrAp.UserService.Application.Interfaces;
using ItTrAp.UserService.Application.DTOs;
using ItTrAp.UserService.Test.Application.Helpers;

namespace ItTrAp.UserService.Test.Application.Commands;

public class CreateUserCommandTest
{
    private static IEnumerable<ValidatorTestData> InvalidFieldTestCases()
    {
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().ToCreateUserDto()),
            true,
            TestName: "ValidUser"
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithFirstName("").ToCreateUserDto()),
            false,
            TestName: "EmptyFirstName",
            FieldName: "User.FirstName",
            ExpectedErrorMessage: "First name is required."
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithFirstName(null).ToCreateUserDto()),
            false,
            TestName: "NullFirstName",
            FieldName: "User.FirstName",
            ExpectedErrorMessage: "First name is required."
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithFirstName("   ").ToCreateUserDto()),
            false,
            TestName: "WhitespaceFirstName",
            FieldName: "User.FirstName",
            ExpectedErrorMessage: "First name is required."
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithLastName("").ToCreateUserDto()),
            false,
            TestName: "EmptyLastName",
            FieldName: "User.LastName",
            ExpectedErrorMessage: "Last name is required."
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithLastName(null).ToCreateUserDto()),
            false,
            TestName: "NullLastName",
            FieldName: "User.LastName",
            ExpectedErrorMessage: "Last name is required."
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithLastName("   ").ToCreateUserDto()),
            false,
            TestName: "WhitespaceLastName",
            FieldName: "User.LastName",
            ExpectedErrorMessage: "Last name is required."
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithEmail("").ToCreateUserDto()),
            false,
            TestName: "EmptyEmail",
            FieldName: "User.Email",
            ExpectedErrorMessage: "Email is required."
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithEmail(null).ToCreateUserDto()),
            false,
            TestName: "NullEmail",
            FieldName: "User.Email",
            ExpectedErrorMessage: "Email is required."
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithEmail("invalid-email").ToCreateUserDto()),
            false,
            TestName: "InvalidEmail",
            FieldName: "User.Email",
            ExpectedErrorMessage: "Valid email is required."
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithEmail("missing@domain").ToCreateUserDto()),
            false,
            TestName: "EmailMissingTld",
            FieldName: "User.Email",
            ExpectedErrorMessage: "Valid email is required."
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithEmail("@nodomain.com").ToCreateUserDto()),
            false,
            TestName: "EmailMissingLocalPart",
            FieldName: "User.Email",
            ExpectedErrorMessage: "Valid email is required."
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithPhone("").ToCreateUserDto()),
            false,
            TestName: "EmptyPhone",
            FieldName: "User.Phone",
            ExpectedErrorMessage: "Phone number is required."
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithPhone(null).ToCreateUserDto()),
            false,
            TestName: "NullPhone",
            FieldName: "User.Phone",
            ExpectedErrorMessage: "Phone number is required."
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithPhone("invalid-phone").ToCreateUserDto()),
            false,
            TestName: "InvalidPhone",
            FieldName: "User.Phone",
            ExpectedErrorMessage: "Phone number must be in a valid international format."
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithPhone("12345").ToCreateUserDto()),
            false,
            TestName: "PhoneMissingPlusPrefix",
            FieldName: "User.Phone",
            ExpectedErrorMessage: "Phone number must be in a valid international format."
        );
        yield return new ValidatorTestData(
            new CreateUserCommand(UserDtoHelper.CreateValidUserDto().WithPhone("+abc123").ToCreateUserDto()),
            false,
            TestName: "PhoneWithLetters",
            FieldName: "User.Phone",
            ExpectedErrorMessage: "Phone number must be in a valid international format."
        );
    }

    [TestCaseSource(nameof(InvalidFieldTestCases))]
    public void CreateUserCommandValidator_InvalidField_ShouldFailValidationWithMessage(ValidatorTestData testData)
    {
        var test = new ValidatorTest<CreateUserCommand,CreateUserCommandValidator>(testData);

        test.Assert();
    }

    [Test]
    public async Task Handle_ValidCommand_ShouldCallServiceAndReturnUserId()
    {
        // Arrange
        var userDto = UserDtoHelper.CreateValidUserDto().ToCreateUserDto();
        var command = new CreateUserCommand(userDto);
        var userServiceMock = new Mock<IUserService>();
        userServiceMock
            .Setup(s => s.CreateUserAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult((uint)123));
        var handler = new CreateUserHandler(userServiceMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(123));
        userServiceMock.Verify(s => s.CreateUserAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
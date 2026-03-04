using ItTrAp.Shared.Test;
using ItTrAp.UserService.Application.Commands;
using ItTrAp.UserService.Application.DTOs;
using ItTrAp.UserService.Application.Interfaces;
using ItTrAp.UserService.Test.Application.Helpers;

namespace ItTrAp.UserService.Test.Application.Commands;

public class UpdateUserCommandTest
{
    private static IEnumerable<ValidatorTestData> InvalidFieldTestCases()
    {
        yield return new ValidatorTestData(
            new UpdateUserCommand(1, UserDtoHelper.CreateValidUserDto().ToUpdateUserDto()),
            true,
            TestName: "ValidUser"
        );
        yield return new ValidatorTestData(
            new UpdateUserCommand(0, UserDtoHelper.CreateValidUserDto().ToUpdateUserDto()),
            false,
            TestName: "InvalidUserId",
            FieldName: "Id",
            ExpectedErrorMessage: "User ID must be greater than 0."
        );
        yield return new ValidatorTestData(
            new UpdateUserCommand(1, UserDtoHelper.CreateValidUserDto().WithFirstName("").ToUpdateUserDto()),
            false,
            TestName: "EmptyFirstName",
            FieldName: "User.FirstName",
            ExpectedErrorMessage: "First name is required."
        );
        yield return new ValidatorTestData(
            new UpdateUserCommand(1, UserDtoHelper.CreateValidUserDto().WithFirstName(null).ToUpdateUserDto()),
            false,
            TestName: "NullFirstName",
            FieldName: "User.FirstName",
            ExpectedErrorMessage: "First name is required."
        );
        yield return new ValidatorTestData(
            new UpdateUserCommand(1, UserDtoHelper.CreateValidUserDto().WithFirstName("   ").ToUpdateUserDto()),
            false,
            TestName: "WhitespaceFirstName",
            FieldName: "User.FirstName",
            ExpectedErrorMessage: "First name is required."
        );
        yield return new ValidatorTestData(
            new UpdateUserCommand(1, UserDtoHelper.CreateValidUserDto().WithLastName("").ToUpdateUserDto()),
            false,
            TestName: "EmptyLastName",
            FieldName: "User.LastName",
            ExpectedErrorMessage: "Last name is required."
        );
        yield return new ValidatorTestData(
            new UpdateUserCommand(1, UserDtoHelper.CreateValidUserDto().WithLastName(null).ToUpdateUserDto()),
            false,
            TestName: "NullLastName",
            FieldName: "User.LastName",
            ExpectedErrorMessage: "Last name is required."
        );
        yield return new ValidatorTestData(
            new UpdateUserCommand(1, UserDtoHelper.CreateValidUserDto().WithLastName("   ").ToUpdateUserDto()),
            false,
            TestName: "WhitespaceLastName",
            FieldName: "User.LastName",
            ExpectedErrorMessage: "Last name is required."
        );
        yield return new ValidatorTestData(
            new UpdateUserCommand(1, UserDtoHelper.CreateValidUserDto().WithPhone("").ToUpdateUserDto()),
            false,
            TestName: "EmptyPhone",
            FieldName: "User.Phone",
            ExpectedErrorMessage: "Phone number is required."
        );
        yield return new ValidatorTestData(
            new UpdateUserCommand(1, UserDtoHelper.CreateValidUserDto().WithPhone(null).ToUpdateUserDto()),
            false,
            TestName: "NullPhone",
            FieldName: "User.Phone",
            ExpectedErrorMessage: "Phone number is required."
        );
        yield return new ValidatorTestData(
            new UpdateUserCommand(1, UserDtoHelper.CreateValidUserDto().WithPhone("   ").ToUpdateUserDto()),
            false,
            TestName: "WhitespacePhone",
            FieldName: "User.Phone",
            ExpectedErrorMessage: "Phone number is required."
        );
        yield return new ValidatorTestData(
            new UpdateUserCommand(1, UserDtoHelper.CreateValidUserDto().WithPhone("invalid-phone").ToUpdateUserDto()),
            false,
            TestName: "InvalidPhoneFormat",
            FieldName: "User.Phone",
            ExpectedErrorMessage: "Phone must be in a valid international format."
        );
        yield return new ValidatorTestData(
            new UpdateUserCommand(1, UserDtoHelper.CreateValidUserDto().WithPhone("12345").ToUpdateUserDto()),
            false,
            TestName: "PhoneMissingPlusPrefix",
            FieldName: "User.Phone",
            ExpectedErrorMessage: "Phone must be in a valid international format."
        );
        yield return new ValidatorTestData(
            new UpdateUserCommand(1, UserDtoHelper.CreateValidUserDto().WithPhone("+abc123").ToUpdateUserDto()),
            false,
            TestName: "PhoneWithLetters",
            FieldName: "User.Phone",
            ExpectedErrorMessage: "Phone must be in a valid international format."
        );
    }

    [TestCaseSource(nameof(InvalidFieldTestCases))]
    public void UpdateUserCommandValidator_InvalidField__ShouldFailValidationWithMessage(ValidatorTestData testData)
    {
        var test = new ValidatorTest<UpdateUserCommand, UpdateUserCommandValidator>(testData);

        test.Assert();
    }

    [Test]
    public async Task Handle_ValidCommand_ShouldCallService()
    {
        var userDto = UserDtoHelper.CreateValidUserDto().ToUpdateUserDto();
        var command = new UpdateUserCommand(1, userDto);
        var userServiceMock = new Mock<IUserService>();
        userServiceMock
            .Setup(s => s.UpdateUserAsync(It.IsAny<uint>(), It.IsAny<UpdateUserDto>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult((uint)123));
        var handler = new UpdateUserHandler(userServiceMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        userServiceMock.Verify(s => s.UpdateUserAsync(It.IsAny<uint>(), It.IsAny<UpdateUserDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
using FluentValidation;
using ItTrAp.Shared.Validators;

namespace ItTrAp.Shared.Test.Validators;

public class PhoneValidatorTest
{
    public record PhoneTestCase(string Phone, bool IsValid);

    public static IEnumerable<PhoneTestCase> PhoneTestCases()
    {
        yield return new PhoneTestCase("+1234567890", true);
        yield return new PhoneTestCase("1234567890", false);
        yield return new PhoneTestCase("+1", false);
        yield return new PhoneTestCase("+1234567890123456", false);
        yield return new PhoneTestCase("+123456789012345", true);
    }

    [TestCaseSource(nameof(PhoneTestCases))]
    public void PhoneNumber_Validation(PhoneTestCase testCase)
    {
        var validator = new InlineValidator<TestModel>();
        validator.RuleFor(x => x.Phone).PhoneNumber();

        var result = validator.Validate(new TestModel { Phone = testCase.Phone });

        result.IsValid.Should().Be(testCase.IsValid);
    }

    private class TestModel
    {
        public required string Phone { get; set; }
    }
}
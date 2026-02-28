using FluentValidation;

namespace ItTrAp.Shared.Validators;

public static class PhoneValidator
{
    private static readonly string PhonePattern = @"^\+[1-9]\d{1,14}$";

    public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Matches(PhonePattern);
    }
}

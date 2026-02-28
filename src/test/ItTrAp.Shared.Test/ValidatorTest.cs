using FluentValidation;
using FluentValidation.Results;
using FluentAssertions;

namespace ItTrAp.Shared.Test;

public class ValidatorTest<T, Validator> where T : class where Validator : AbstractValidator<T>, new()
{
    private readonly ValidatorTestData _data;
    private readonly IValidator<T> _validator;
    private readonly ValidationResult _result;

    public ValidatorTest(ValidatorTestData data)
    {
        _data = data;
        _validator = new Validator();

        _result = _validator.Validate((T)_data.Request);
    }

    public void Assert()
    {
        if (!_data.IsValid)
        {
            _result.IsValid.Should().BeFalse();
            _result.Errors.Should().ContainSingle(e => e.PropertyName == _data.FieldName && e.ErrorMessage == _data.ExpectedErrorMessage);
        }
        else
        {
            _result.IsValid.Should().BeTrue();
        }
    }
}

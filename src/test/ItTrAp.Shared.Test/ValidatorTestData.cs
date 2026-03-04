using FluentValidation;
using MediatR;

namespace ItTrAp.Shared.Test;

public record ValidatorTestData
(
    IBaseRequest Request,
    bool IsValid,
    string? TestName = null,
    string? FieldName = null,
    string ExpectedErrorMessage = ""
)
{
    public override string ToString() => TestName ?? FieldName ?? "UnnamedTest";
}
using FluentValidation;

namespace SharedInfrastructure.Extensions;

public static class ValidatorExtensions
{
    public static IRuleBuilder<T, TProperty> ValidId<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, string identifierName = "Id")
    {
        return ruleBuilder
            .NotEmpty().WithMessage($"{identifierName} is required.");
    }
}

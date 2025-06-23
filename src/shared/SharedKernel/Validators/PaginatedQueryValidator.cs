using FluentValidation;
using SharedKernel.Queries;

namespace SharedKernel.Validators;

public abstract class PaginatedQueryValidator<TQuery, TItem> : AbstractValidator<TQuery>
    where TQuery : PaginatedQuery<TItem>
{
    protected PaginatedQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.");
    }
}
using FluentValidation;
using SharedInfrastructure.Extensions;

namespace UserService.Application.Features.Users.Queries.GetById;

public sealed class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id).ValidId();
    }
}

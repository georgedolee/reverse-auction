using FluentValidation;
using SharedInfrastructure.Extensions;

namespace UserService.Application.Features.Roles.Queries.GetById;

public sealed class GetRoleByIdQueryValidator : AbstractValidator<GetRoleByIdQuery>
{
    public GetRoleByIdQueryValidator()
    {
        RuleFor(x => x.Id).ValidId();
    }
}

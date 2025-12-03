using FluentValidation;

namespace SchoolAccount.Application.Teams.CreateTeam;

internal class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress().MaximumLength(250);
        RuleFor(x => x.Acronym).MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1500);
    }
}

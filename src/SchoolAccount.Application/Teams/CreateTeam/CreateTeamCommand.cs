using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Teams.CreateTeam;

public sealed record CreateTeamCommand(string Name, string EmailAddress, string Description, string? Acronym)
    : ICommand<long>;

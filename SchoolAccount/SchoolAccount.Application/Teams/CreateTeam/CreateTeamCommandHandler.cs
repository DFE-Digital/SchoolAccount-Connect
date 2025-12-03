using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Domain.Teams;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Teams.CreateTeam;

public class CreateTeamCommandHandler(ITeamWriteStore teamWriteStore)
    : ICommandHandler<CreateTeamCommand, long>
{
    public async Task<Result<long>> Handle(
        CreateTeamCommand command,
        CancellationToken cancellationToken
    )
    {
        var team = Team.Create(
            command.Name,
            command.EmailAddress,
            command.Description,
            command.Acronym
        );
        return await teamWriteStore.CreateTeamAsync(team, cancellationToken);
    }
}

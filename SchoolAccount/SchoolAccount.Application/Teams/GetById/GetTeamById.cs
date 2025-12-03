using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Teams.GetById;

public class GetTeamById : IQuery<TeamResponse>
{
    public long Id { get; init; }
}

using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.SubTask;

public sealed record GetSubTasksForCardsQuery(DateOnly? QueryFrom = null) : IQuery<GetSubTasksForCardsResponse>;
using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Feedback.Commands;

public sealed record RecordBannerExposureCommand(string PageId) : ICommand;

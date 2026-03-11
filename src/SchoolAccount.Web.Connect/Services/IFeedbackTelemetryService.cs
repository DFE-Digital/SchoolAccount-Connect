using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Services;

public interface IFeedbackTelemetryService
{
    void RecordPageFeedback(PageFeedbackRequest request);
}
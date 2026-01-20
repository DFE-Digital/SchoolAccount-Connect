namespace SchoolAccount.Web.Connect.Models;

public sealed class ErrorViewModel
{
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public string? RequestId { get; init; }


    public bool ShowException { get; init; }
    public Exception? Exception { get; set; }
    
    public string? Heading { get; set; }
    public IEnumerable<string> Messages { get; set; } = [];
    
    public string? OriginalPath { get; set; }
}

namespace SchoolAccount.Kernel;

public record Error(string Code, string Description, string? Property = null);

public record Failure(string Code, string Description, string? Property = null) 
    : Error(Code, Description, Property);

public record Validation(string Code, string Description, string? Property = null) 
    : Error(Code, Description, Property);

public record NotFound(string Code, string Description, string? Property = null) 
    : Error(Code, Description, Property);

public record Conflict(string Code, string Description, string? Property = null) 
    : Error(Code, Description, Property);
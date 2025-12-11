namespace SchoolAccount.Kernel;

public interface IAutocomplete
{
    Uri Endpoint { get; init; }
    
    long? Value { get; set; }
    string DisplayValue { get; set; }
}
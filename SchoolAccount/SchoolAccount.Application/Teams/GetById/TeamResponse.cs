namespace SchoolAccount.Application.Teams.GetById;

public class TeamResponse
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string DirectorateName { get; set; } = string.Empty;
}
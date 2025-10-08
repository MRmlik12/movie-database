namespace MovieDatabase.Api.Core.Documents;

public abstract class BaseDocument
{
    public Guid Id { get; protected init; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
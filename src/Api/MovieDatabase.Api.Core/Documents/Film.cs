namespace MovieDatabase.Api.Core.Documents;

public class Film : BaseDocument
{
    public string Title { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    public Director Director { get; set; }
    public List<Actor> Actors { get; set; }
    public List<Genre> Genres { get; set; }
    public Producer Producer { get; set; }
    public string? Description { get; set; }
}
namespace MovieDatabase.Api.Core.Documents;

public class Film : BaseDocument
{
    public string Title { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string Director { get; set; }
    public Actor[] Actors { get; set; }
    public Genre Genre { get; set; }
}
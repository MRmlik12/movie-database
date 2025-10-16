namespace MovieDatabase.Api.Core.Documents.Films;

public class Genre : BaseDocument
{
    public string Name { get; set; }

    public Genre(string? id, string name)
    {
        if (id is not null)
        {
            Id = Guid.Parse(id);
        }

        Name = name;
    }
}
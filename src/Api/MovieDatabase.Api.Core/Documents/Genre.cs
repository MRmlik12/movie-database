using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api.Core.Documents;

public class Genre : BaseDocument, IFrom<Genre, GenreDto>
{
    public string Name { get; set; }

    public Genre(string id, string name)
    {
        Id = Guid.Parse(id);
        Name = name;
    }

    public static Genre From(GenreDto document)
        => new Genre(document.Id, document.Name);
}
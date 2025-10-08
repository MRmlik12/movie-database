using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Core.Interfaces;

namespace MovieDatabase.Api.Core.Documents;

public class Genre : BaseDocument, IFrom<Genre, GenreDto>
{
    public string Name { get; set; }

    public Genre(string name)
    {
        Name = name;
    }
    
    private Genre(string id, string name)
    {
        Id = Guid.Parse(id);
        Name = name;
    }

    public static Genre From(GenreDto document)
        => new (document.Id, document.Name);
}
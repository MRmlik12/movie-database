namespace MovieDatabase.Api.Core.Documents;

public class Actor : BaseDocument
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public List<string> FilmIds { get; set; } = new();

    public Actor(string? id, string name, string surname)
    {
        if (id is not null)
        {
            Id = Guid.Parse(id);
        }
        
        Name = name;
        Surname = surname;
    }
}
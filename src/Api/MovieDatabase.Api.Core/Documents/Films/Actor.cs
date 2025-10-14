namespace MovieDatabase.Api.Core.Documents.Films;

public class Actor : BaseDocument
{
    public string Name { get; set; }
    public string Surname { get; set; }

    public Actor(string? id, string name, string surname)
    {
        if (id is not null)
        {
            Id = Guid.Parse(id);
        }
        
        Name = name;
        Surname = surname;
    }

    public override string ToString()
    {
        return $"{Name} {Surname}";
    }
}
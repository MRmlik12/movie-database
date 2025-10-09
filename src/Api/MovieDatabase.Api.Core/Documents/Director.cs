namespace MovieDatabase.Api.Core.Documents;

public class Director : BaseDocument
{
    public Director(string? id, string name, string surname)
    {
        if (id is not null)
        {
            Id = Guid.Parse(id);
        }
        
        Name = name;
        Surname = surname;
    }

    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
}
namespace MovieDatabase.Api.Core.Documents.Films;

public class DirectorInfo : BaseDocument
{
    public DirectorInfo(string? id, string name, string surname)
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
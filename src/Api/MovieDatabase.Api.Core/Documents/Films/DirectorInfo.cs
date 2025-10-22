namespace MovieDatabase.Api.Core.Documents.Films;

public class DirectorInfo : BaseDocument
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
}
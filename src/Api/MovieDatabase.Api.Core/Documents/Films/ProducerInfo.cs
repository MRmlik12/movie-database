namespace MovieDatabase.Api.Core.Documents.Films;

public class ProducerInfo : BaseDocument
{
    public string Name { get; set; }

    public ProducerInfo(string? id, string name)
    {
        if (id is not null)
        {
            Id = Guid.Parse(id);
        }
        
        Name = name;
    }
}

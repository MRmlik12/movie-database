namespace MovieDatabase.Api.Core.Documents;

public class Producer : BaseDocument
{
    public string Name { get; set; }

    public Producer(string id, string name)
    {
        Id = Guid.Parse(id);
        Name = name;
    }
}

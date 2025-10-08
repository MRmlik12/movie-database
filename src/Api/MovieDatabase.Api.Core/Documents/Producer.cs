using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Core.Interfaces;

namespace MovieDatabase.Api.Core.Documents;

public class Producer : BaseDocument, IFrom<Producer, ProducerDto>
{
    public string Name { get; set; }

    private Producer(string id, string name)
    {
        Id = Guid.Parse(id);
        Name = name;
    }

    public static Producer From(ProducerDto document)
        => new(document.Id, document.Name);
}

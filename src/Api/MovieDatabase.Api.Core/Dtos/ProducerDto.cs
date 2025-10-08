using MovieDatabase.Api.Core.Documents;
using MovieDatabase.Api.Core.Interfaces;

namespace MovieDatabase.Api.Core.Dtos;

public record ProducerDto(string Id, string Name) : IFrom<ProducerDto, Producer>
{
    public static ProducerDto From(Producer document)
        => new (document.Id.ToString(), document.Name);
}
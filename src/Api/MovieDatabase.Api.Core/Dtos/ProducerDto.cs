using MovieDatabase.Api.Core.Documents;
using MovieDatabase.Api.Core.Interfaces;

namespace MovieDatabase.Api.Core.Dtos;

public record ProducerDto(string Id, string Name) : IFrom<ProducerDto, ProducerInfo>
{
    public static ProducerDto From(ProducerInfo document)
        => new (document.Id.ToString(), document.Name);
}
using System.Text.Json.Serialization;

namespace MovieDatabase.Api.Core.Documents;

public class Film : BaseDocument
{
    [JsonIgnore]
    public const string PartitionKey = "/title";
    
    public string Title { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    public string DirectorId { get; set; } = null!;
    public string ProducerId { get; set; } = null!;
    public DirectorInfo Director { get; set; }
    public List<Actor> Actors { get; set; }
    public List<Genre> Genres { get; set; }
    public ProducerInfo Producer { get; set; }
    public string? Description { get; set; }
}
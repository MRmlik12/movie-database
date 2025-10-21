using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MovieDatabase.Api.Core.Documents.Users;

public class User : BaseDocument
{
    [JsonIgnore]
    public const string PartitionKey = "/email";

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    [JsonConverter(typeof(StringEnumConverter))]
    public UserRoles Role { get; set; }
}
using System.Text;

using Microsoft.Azure.Cosmos;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace MovieDatabase.Api.Infrastructure.Db;

public class NewtonsoftJsonCosmosSerializer : CosmosSerializer
{
    private readonly JsonSerializer _serializer = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Converters =
        {
            new StringEnumConverter(new CamelCaseNamingStrategy(), false)
        }
    };

    public override T FromStream<T>(Stream stream)
    {
        using (stream)
        {
            if (typeof(Stream).IsAssignableFrom(typeof(T)))
            {
                return (T)(object)(stream);
            }

            using StreamReader sr = new(stream);
            using JsonTextReader jsonTextReader = new(sr);

            return _serializer.Deserialize<T>(jsonTextReader);
        }
    }

    public override Stream ToStream<T>(T input)
    {
        var streamPayload = new MemoryStream();

        using var streamWriter = new StreamWriter(streamPayload, encoding: Encoding.Default, bufferSize: 1024, leaveOpen: true);
        using var writer = new JsonTextWriter(streamWriter);

        writer.Formatting = Formatting.None;
        _serializer.Serialize(writer, input);

        writer.Flush();
        streamWriter.Flush();

        streamPayload.Position = 0;

        return streamPayload;
    }
}
namespace MovieDatabase.IntegrationTests.Helpers;

public class GraphQLResponse<T>
{
    public T? Data { get; set; }
    public GraphQLError[]? Errors { get; set; }
}


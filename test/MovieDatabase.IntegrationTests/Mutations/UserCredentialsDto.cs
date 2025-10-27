namespace MovieDatabase.IntegrationTests.Mutations;

public class UserCredentialsDto
{
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public string? Token { get; set; }
    public DateTime? ExpireTime { get; set; }
}


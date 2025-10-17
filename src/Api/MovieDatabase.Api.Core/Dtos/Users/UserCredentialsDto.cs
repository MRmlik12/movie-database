﻿using MovieDatabase.Api.Core.Documents.Users;
using MovieDatabase.Api.Core.Interfaces;

namespace MovieDatabase.Api.Core.Dtos.Users;

public record UserCredentialsDto(string Id, string Username, string Email, string? Role) : IFrom<UserCredentialsDto, User>
{
    public string? Token { get; set; }
    public DateTime? ExpireTime { get; set; }
    
    public static UserCredentialsDto From(User from)
        => new(
            from.Id.ToString(),
            from.Username,
            from.Email,
            Enum.GetName(from.Role)
        );
}

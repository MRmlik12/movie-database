using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Core.Services;

namespace MovieDatabase.Api.Types;

[ExtendObjectType(typeof(Film))]
public sealed class FilmTypeExtensions
{
    public string? ThumbnailUrl([Parent] Film film, [Service] IBlobService blobService)
        => film.Thumbnail is null ? null : blobService.GetContentUrl(film.Thumbnail);
}

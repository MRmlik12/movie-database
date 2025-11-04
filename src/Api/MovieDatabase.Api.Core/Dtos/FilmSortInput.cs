namespace MovieDatabase.Api.Core.Dtos;

public enum FilmSortField
{
    Title,
    ReleaseDate
}

public enum SortDirection
{
    Ascending,
    Descending
}

public record FilmSortInput(
    FilmSortField Field = FilmSortField.Title,
    SortDirection Direction = SortDirection.Ascending
);


using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Infrastructure.Db;

namespace MovieDatabase.Api;

public class Query
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Film> Films(
        [Service] AppDbContext dbContext)
        => dbContext.Films;
}
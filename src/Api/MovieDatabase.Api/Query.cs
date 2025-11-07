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
    
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Actor> Actors(
        [Service] AppDbContext dbContext)
    {
        return dbContext.Films
            .SelectMany(f => f.Actors)
            .DistinctBy(a => new { a.Name, a.Surname });
    }
    
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Genre> Genres(
        [Service] AppDbContext dbContext)
    {
        return dbContext.Films
            .SelectMany(f => f.Genres)
            .DistinctBy(g => g.Name);
    }
    
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<DirectorInfo> Directors(
        [Service] AppDbContext dbContext)
    {
        return dbContext.Films
            .Select(f => f.Director)
            .DistinctBy(d => new { d.Name, d.Surname });
    }

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ProducerInfo> Producers(
        [Service] AppDbContext dbContext)
    {
        return dbContext.Films
            .Select(f => f.Producer)
            .DistinctBy(p => p.Name);
    }
}
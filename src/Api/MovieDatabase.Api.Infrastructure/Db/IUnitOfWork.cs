namespace MovieDatabase.Api.Infrastructure.Db;

public interface IUnitOfWork
{
    Task Commit();
}
namespace MovieDatabase.Api.Core.Dtos;

public interface IFrom<out TOut, in TIn>
    where TIn : class
{
    static abstract TOut From(TIn document);    
}

namespace MovieDatabase.Api.Core.Interfaces;

public interface IFrom<out TOut, in TIn>
    where TIn : class
{
    static abstract TOut From(TIn from);
}
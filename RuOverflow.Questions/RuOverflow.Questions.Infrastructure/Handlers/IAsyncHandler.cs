namespace RuOverflow.Questions.Infrastructure.Handlers;

public interface IAsyncHandler<in TIn> : IHandler<TIn, Task>
{
}

public interface IAsyncHandler<in TIn, TOut> : IHandler<TIn, Task<TOut>>
{
}

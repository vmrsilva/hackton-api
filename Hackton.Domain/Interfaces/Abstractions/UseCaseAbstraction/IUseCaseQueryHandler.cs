namespace Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction
{
    public interface IUseCaseQueryHandler<TCommand, TResponse>
    {
        Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken = default);
    }
}

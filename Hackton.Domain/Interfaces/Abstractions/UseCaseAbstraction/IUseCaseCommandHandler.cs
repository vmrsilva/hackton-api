namespace Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction
{
    public interface IUseCaseCommandHandler<TCommand>
    {
        Task Handle(TCommand command, CancellationToken cancellation = default);
    }
}

using Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction;

namespace Hackton.Domain.Interfaces.Video.UseCases
{
    public interface IPostNewVideoUseCaseHandler<TCommand> : IUseCaseCommandHandler<TCommand>
    {
    }
}

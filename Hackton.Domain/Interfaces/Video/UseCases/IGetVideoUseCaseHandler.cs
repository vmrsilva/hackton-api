using Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction;

namespace Hackton.Domain.Interfaces.Video.UseCases
{
    public interface IGetVideoUseCaseHandler<TCommand, TResponse> : IUseCaseQueryHandler<TCommand, TResponse>
    {
    }
}

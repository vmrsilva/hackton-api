using Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction;

namespace Hackton.Domain.Interfaces.VideoResult.UseCase
{
    public interface IGetVideoResultsUseCaseHandler<TCommand, TResponse> : IUseCaseQueryHandler<TCommand, TResponse>
    {
    }
}

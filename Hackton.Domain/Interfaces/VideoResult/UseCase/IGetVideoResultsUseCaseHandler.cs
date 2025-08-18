using Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackton.Domain.Interfaces.VideoResult.UseCase
{
    public interface IGetVideoResultsUseCaseHandler<TCommand, TResponse> : IUseCaseQueryHandler<TCommand, TResponse>
    {
    }
}

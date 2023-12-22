using MediatR;

namespace Veneta.Recruitment.ConsumerService.Dependencies;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : ICommand
{
}
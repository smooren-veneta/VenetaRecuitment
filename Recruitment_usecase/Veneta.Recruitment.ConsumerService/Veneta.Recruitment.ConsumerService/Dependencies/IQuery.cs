using MediatR;

namespace Veneta.Recruitment.ConsumerService.Dependencies;

public interface IQuery<out T> : IRequest<T>;
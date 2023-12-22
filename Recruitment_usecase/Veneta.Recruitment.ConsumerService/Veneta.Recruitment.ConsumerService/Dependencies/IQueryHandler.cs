using MediatR;

namespace Veneta.Recruitment.ConsumerService.Dependencies;

public interface IQueryHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
    where TRequest : IQuery<TResponse>
    where TResponse : class?;
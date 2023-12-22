using MediatR;
using Microsoft.AspNetCore.Mvc;
using Veneta.Recruitment.ConsumerService.Queries.GetConsumer;
using Veneta.Recruitment.ConsumerService.Queries.GetConsumers;
using Veneta.Recruitment.ConsumerService.Requests;

namespace Veneta.Recruitment.ConsumerService;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        _ = new ConsumerEndpoints().Map(endpoints);

        return endpoints;
    }
}

internal class ConsumerEndpoints
{
    public IEndpointRouteBuilder Map(IEndpointRouteBuilder endpoints)
    {
        var consumersGroup = "/consumers";

        endpoints
            .MapGroup(consumersGroup)
            .MapGet("/{id:guid}", GetConsumer);

        endpoints
            .MapGroup(consumersGroup)
            .MapGet(String.Empty, GetConsumers);

        endpoints
            .MapGroup(consumersGroup)
            .MapPost(String.Empty, CreateConsumer);

        endpoints
            .MapGroup(consumersGroup)
            .MapPut("/{id:guid}", UpdateConsumer);

        return endpoints;
    }

    private async Task<IResult> GetConsumer(
        [FromServices] IMediator mediator,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetConsumerQuery()
        {
            ConsumerId = id
        };

        var consumer = await mediator.Send(query, cancellationToken);
        return consumer is null ? Results.NotFound() : Results.Ok(consumer);
    }

    private async Task<IResult> GetConsumers(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var consumers = await mediator.Send(new GetConsumersQuery(), cancellationToken);
        return Results.Ok(consumers);
    }

    private async Task<IResult> CreateConsumer(
        [FromServices] IMediator mediator,
        [FromBody] CreateConsumerRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = request.ToCommand();

            await mediator.Send(command, cancellationToken);

            return Results.Ok();
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }

    private async Task<IResult> UpdateConsumer(
        [FromServices] IMediator mediator,
        [FromBody] UpdateConsumerRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = request.ToCommand(id);

            await mediator.Send(command, cancellationToken);

            return Results.Ok();
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }
}

using Ordering.Application.Orders.Commands.DeleteOrder;
using Ordering.Domain.VavlueObjects;
using static Ordering.API.Endpoints.UpdateOrder;

namespace Ordering.API.Endpoints;
public class DeleteOrder : ICarterModule
{
    //public record DeleteOrderRequest(Guid Id);
    public record DeleteOrderResponse(bool IsSuccess);

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id}", async (Guid Id, ISender sender) =>
        {
            var command = new DeleteOrderCommand(Id);
            var result = await sender.Send(command);
            var response = result.Adapt<DeleteOrderResponse>();

            return Results.Ok(response);
        })
        .WithName("DeleteOrder")
        .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Order")
        .WithDescription("Delete Order");
    }
}

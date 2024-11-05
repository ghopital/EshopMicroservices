
using Catalog.API.Products.GetProducts;

namespace Catalog.API.Products.GetProductById
{
    //public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResponse(Product Product);

    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{Id}", async (Guid Id,ISender sender) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(Id));
                var response = result.Adapt<GetProductByIdResponse>();
                return Results.Ok(response);
            }).WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id");
        }
    }
}

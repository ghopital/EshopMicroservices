
namespace Catalog.API.Products.GetProductByCategory
{
    //public record GetProductByCategoryRequest(string Category) : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);

    public class GetProductbyCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
            {
                var results = await sender.Send(new GetProductByCategoryQuery(category));
                var response = results.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(response);
            }).WithName("GetProductsByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products by Category")
            .WithDescription("Get Products by Category");
        }
    }
}

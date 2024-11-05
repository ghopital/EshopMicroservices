
namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id):ICommand<DeleteProductResult>;

    public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
        }
    }
    public class DeleteProductCommandHandler(IDocumentSession session/*, ILogger<GetProductbyCategoryQueryHandler> logger*/) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            //logger.LogInformation("DeleteProductCommandHandler.Handle called with {@Query}", command);

            var product = await session.LoadAsync<Product>(command.Id);
            if(product == null)
            {
                throw new ProductNotFoundException(command.Id);
            }

            session.Delete(product);
            await session.SaveChangesAsync();
            return new DeleteProductResult(true);
        }
    }
}

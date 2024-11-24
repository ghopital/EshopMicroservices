
using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class StoreBaskeCommandtHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProtoService) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        ShoppingCart cart = command.Cart;
        await DeductDiscount(cart, cancellationToken);

        //Store in Database
        await repository.StoreBasket(cart, cancellationToken);
        //Update Cache

        return new StoreBasketResult(cart.UserName);
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        //Comunicate with Discount GRPC 
        foreach (var item in cart.Items)
        {
            var request = new GetDiscountRequest { ProductName = item.ProductName };
            var coupon = await discountProtoService.GetDiscountAsync(request, cancellationToken: cancellationToken);
            item.Price -= coupon.Amount;
        };
    }
}

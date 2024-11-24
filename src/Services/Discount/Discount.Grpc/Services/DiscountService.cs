using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbcontext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbcontext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
        if (coupon == null)
        {
            coupon = new Coupon { ProductName = "No discount", Amount = 0, Description = "No discount desc" };
        }
        logger.LogInformation("Discount is retrieved for ProductName: {productName}, Amount: {amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));
        }

        dbcontext.Coupons.Add(coupon);
        await dbcontext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully created. ProductName: {productName}, Amount: {amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbcontext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={ request.ProductName } is not found."));
        }

        dbcontext.Coupons.Remove(coupon);
        await dbcontext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully delete. ProductName: {productName}, Amount: {amount}", coupon.ProductName, coupon.Amount);

        var response = new DeleteDiscountResponse() { Success = true };
        return response;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));
        }

        var couponToUpdate = await dbcontext.Coupons.FirstOrDefaultAsync(x => x.Id == coupon.Id);
        if (couponToUpdate is null) { throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request")); }

        couponToUpdate.ProductName = coupon.ProductName;
        couponToUpdate.Description = coupon.Description;
        couponToUpdate.Amount = coupon.Amount;

        dbcontext.Coupons.Update(couponToUpdate);
        await dbcontext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully updated. ProductName: {productName}, Amount: {amount}", coupon.ProductName, coupon.Amount);

        var couponModel = couponToUpdate.Adapt<CouponModel>();
        return couponModel;
    }
}


using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Ordering.Application.Orders.Queries.GetOrders;
public class GetOrdersHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

        var orders = await dbContext.Orders.Include(o => o.OrderItems).AsNoTracking()
            .OrderBy(o => o.OrderName.Value)
            .Skip(pageSize * pageIndex).Take(pageSize).ToListAsync(cancellationToken);

        var paginatedResult = new PaginatedResult<OrderDto>(pageIndex, pageSize, totalCount, orders.ToOrderDtoList());

        return new GetOrdersResult(paginatedResult);
    }
}

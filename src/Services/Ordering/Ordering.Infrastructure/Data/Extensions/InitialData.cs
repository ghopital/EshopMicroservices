using Ordering.Domain.Models;
using Ordering.Domain.VavlueObjects;
using System.Net.NetworkInformation;

namespace Ordering.Infrastructure.Data.Extensions;
internal class InitialData
{
    public static IEnumerable<Customer> Customers =>
        new List<Customer>
        {
            Customer.Create(CustomerId.Of(new Guid("2ca3d2fa-86d0-4e07-ab48-be6af7f3fecc")),"Gabriel","ghopital@gmail.com"),
            Customer.Create(CustomerId.Of(new Guid("475f40a9-d45c-4368-ae7d-0fca50eb5737")),"Nicolas","ghopital@hotmail.com"),
        };

    public static IEnumerable<Product> Products =>
        new List<Product>
        {
            Product.Create(ProductId.Of(new Guid("9b0aa84f-e102-4740-b2c3-7d767d918104")),"IPhone X",500),
            Product.Create(ProductId.Of(new Guid("e502a71c-8be5-4ca3-84eb-057db92ff13a")),"Samsung 10",400),
            Product.Create(ProductId.Of(new Guid("514c18d6-bbdd-4200-a3e2-eb4e814f8809")),"Huawei Plus",650),
            Product.Create(ProductId.Of(new Guid("73ac3970-673d-4c5b-90cc-acb6cf35a230")),"Xiaomi Mi",450),
        };

    public static IEnumerable<Order> Orders
    {
        get
        {
            var address1 = Address.Of("Gabriel", "Hopital", "ghopital@gmail.com", "Ana Belen 19", "Spain", "Malaga", "29631");
            var address2 = Address.Of("Nicolas", "Hopital", "ghopital@hotmail.com", "Boccuzzi 559", "Argentina", "Buenos Aires", "1888");

            var payment1 = Payment.Of("Gabriel", "5555555555554444", "12/28", "355", 1);
            var payment2 = Payment.Of("Nicolas", "8888555555444444", "06/30", "222", 2);

            var order1 = Order.Create(OrderId.Of(Guid.NewGuid())
                , CustomerId.Of(new Guid("2ca3d2fa-86d0-4e07-ab48-be6af7f3fecc"))
                , OrderName.Of("ORD_001")
                , shippingAddress: address1
                , billingAddress: address1
                , payment1);

            order1.Add(ProductId.Of(new Guid("9b0aa84f-e102-4740-b2c3-7d767d918104")), 2, 500);
            order1.Add(ProductId.Of(new Guid("e502a71c-8be5-4ca3-84eb-057db92ff13a")), 1, 400);

            var order2 = Order.Create(OrderId.Of(Guid.NewGuid())
                , CustomerId.Of(new Guid("475f40a9-d45c-4368-ae7d-0fca50eb5737"))
                , OrderName.Of("ORD_002")
                , shippingAddress: address2
                , billingAddress: address2
                , payment2);

            order2.Add(ProductId.Of(new Guid("514c18d6-bbdd-4200-a3e2-eb4e814f8809")), 1, 650);
            order2.Add(ProductId.Of(new Guid("73ac3970-673d-4c5b-90cc-acb6cf35a230")), 2, 450);

            return new List<Order>
            {
                order1, order2
            };
        }
    }
}

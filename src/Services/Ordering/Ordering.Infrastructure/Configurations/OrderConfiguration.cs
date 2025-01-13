using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;

namespace Ordering.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasConversion(
            orderId => orderId.Value,
            dbId => OrderId.Of(dbId));
        builder.HasOne<Customer>().WithMany().HasForeignKey(o => o.CustomerId).IsRequired();

        builder.HasMany(o => o.OrderItems).WithOne().HasForeignKey(oi => oi.OrderId);

        builder.ComplexProperty(o => o.OrderName, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value).HasColumnName(nameof(Order.OrderName)).HasMaxLength(100).IsRequired();
        });

        builder.ComplexProperty(o => o.ShippingAddress, shippingAddressBuilder =>
        {
            shippingAddressBuilder.Property(s => s.FirstName).HasMaxLength(50).IsRequired();
            shippingAddressBuilder.Property(s => s.LastName).HasMaxLength(50).IsRequired();
            shippingAddressBuilder.Property(s => s.EmailAddress).HasMaxLength(50);
            shippingAddressBuilder.Property(s => s.AddressLine).HasMaxLength(180).IsRequired();
            shippingAddressBuilder.Property(s => s.Country).HasMaxLength(50);
            shippingAddressBuilder.Property(s => s.State).HasMaxLength(50);
            shippingAddressBuilder.Property(s => s.ZipCode).HasMaxLength(5).IsRequired();
        });

        builder.ComplexProperty(o => o.BillingAddress, billingAddressBuilder =>
        {
            billingAddressBuilder.Property(b => b.FirstName).HasMaxLength(50).IsRequired();
            billingAddressBuilder.Property(b => b.LastName).HasMaxLength(50).IsRequired();
            billingAddressBuilder.Property(b => b.EmailAddress).HasMaxLength(50);
            billingAddressBuilder.Property(b => b.AddressLine).HasMaxLength(180).IsRequired();
            billingAddressBuilder.Property(b => b.Country).HasMaxLength(50);
            billingAddressBuilder.Property(b => b.State).HasMaxLength(50);
            billingAddressBuilder.Property(b => b.ZipCode).HasMaxLength(5).IsRequired();
        });

        builder.ComplexProperty(o => o.Payment, paymentBuilder =>
        {
            paymentBuilder.Property(p => p.CardNumber).HasMaxLength(24).IsRequired();
            paymentBuilder.Property(p => p.CardName).HasMaxLength(50);
            paymentBuilder.Property(p => p.Expiration).HasMaxLength(10);
            paymentBuilder.Property(p => p.CVV).HasMaxLength(3);
            paymentBuilder.Property(p => p.PaymentMethod);
        });

        builder.Property(o => o.Status).HasDefaultValue(OrderStatus.Draft).HasConversion(s => s.ToString(),
            dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

        builder.Property(o => o.TotalPrice);
    }
}

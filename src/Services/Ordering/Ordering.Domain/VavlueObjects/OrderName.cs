using System.Net.Http.Headers;

namespace Ordering.Domain.VavlueObjects;
public record OrderName
{
    public string Value { get; }
    private const int DefaultLenght = 7;
    private OrderName(string value) => Value = value;
    public static OrderName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DefaultLenght);
        return new OrderName(value);
    }
}

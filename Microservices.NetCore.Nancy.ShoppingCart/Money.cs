namespace Microservices.NetCore.Nancy.ShoppingCart;

public class Money(string currency, decimal amount)
{
    public string Currency { get; } = currency;
    public decimal Amount { get; } = amount;
}
using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects;

public class Money : ValueObject
{
    public double Amount { get; set; }
    //private string Currency  { get; set; }

    public Money(double amount)
    {
        Amount = amount;
    }

    // public Money(decimal amount, string currency)
    // {
    //     if (amount < 0)
    //     {
    //         throw new ArgumentException("Amount must be positive number");
    //     }
    //     Amount = amount;
    //     Currency = currency;
    // }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        // yield return Currency;
    }
}
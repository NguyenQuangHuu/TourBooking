using QuanLySanPham.Domain.Exceptions;

namespace QuanLySanPham.Domain.Commons;

public abstract class EntityId : ValueObject
{
    public Guid Value { get; }

    protected EntityId(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException($"{GetType().Name} cannot be empty");
        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects;

public class PoiType : ValueObject
{
    public static PoiType Dining => new("ĂN UỐNG");
    public static PoiType SightSeeing => new("THAM QUAN");
    public static PoiType Entertainment => new("VUI CHƠI");
    public string Value { get; set; }

    private PoiType(string value)
    {
        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static PoiType From(string poiType)
    {
        return poiType.Trim().ToUpperInvariant() switch
        {
            "ĂN UỐNG" => Dining,
            "THAM QUAN" => SightSeeing,
            "VUI CHƠI" => Entertainment,
            _ => throw new ArgumentException($"Invalid product type: {poiType}")
        };
    }
}
using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects.Ids;

public class EmployeeId : EntityId
{
    public EmployeeId(Guid value) : base(value)
    {
    }

    public static EmployeeId From(Guid id)
    {
        return new EmployeeId(id);
    }
}
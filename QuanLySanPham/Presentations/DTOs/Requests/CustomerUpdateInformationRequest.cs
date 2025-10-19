using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Presentations.DTOs.Requests;

public record CustomerUpdateInformationRequest(string DisplayName,DateOnly DateOfBirth,Gender Gender,
    string IdentityCardNumber,string Address);
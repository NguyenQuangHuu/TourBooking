namespace QuanLySanPham.Application.Services;

public interface IPasswordHasher
{
    string Hash(string password);
    bool VerifyPassword(string password, string hashPassword);
}
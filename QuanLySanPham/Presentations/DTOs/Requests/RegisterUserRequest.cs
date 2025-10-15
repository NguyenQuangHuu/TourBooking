namespace QuanLySanPham.Presentations.DTOs.Requests;

public record RegisterUserRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public RegisterUserRequest(string username, string password, string email, string phoneNumber)
    {
        Username = username;
        Password = password;
        Email = email;
        PhoneNumber = phoneNumber;
    }
}
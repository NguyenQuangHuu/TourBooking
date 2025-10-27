using System.ComponentModel.DataAnnotations;

namespace QuanLySanPham.Presentations.DTOs.DTO;

public class PassengerDto
{
    public string FullName { get; set; }
    public DateOnly BirthDate { get; set; }
    public string IdentityNo { get; set; }
    public string PassportNo { get; set; }
    public string Gender { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }

    public PassengerDto(string fullName, DateOnly birthDate, string identityNo, string passportNo,string gender, string phoneNumber, string email, string address)
    {
        FullName = fullName;
        BirthDate = birthDate;
        IdentityNo = identityNo;
        PassportNo = passportNo;
        Gender = gender;
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
    }
}
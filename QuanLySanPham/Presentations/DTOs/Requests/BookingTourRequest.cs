using System.ComponentModel.DataAnnotations;

namespace QuanLySanPham.Presentations.DTOs.Requests;

public class BookingTourRequest
{
    [Required]
    public Guid TourInstanceId { get; set; }
    [Required]
    public int TotalSlots { get; set; }

}
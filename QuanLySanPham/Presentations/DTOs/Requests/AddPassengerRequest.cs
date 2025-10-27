using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuanLySanPham.Presentations.DTOs.DTO;

namespace QuanLySanPham.Presentations.DTOs.Requests;

public class AddPassengerRequest
{
    public List<PassengerDto> Passengers { get; set; }

    public AddPassengerRequest(List<PassengerDto> passengers)
    {
        Passengers = passengers;
    }
}
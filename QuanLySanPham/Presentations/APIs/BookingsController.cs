using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLySanPham.Domain.Aggregates.Bookings;
using QuanLySanPham.Presentations.DTOs.Requests;

namespace QuanLySanPham.Presentations.APIs;
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BookingsController : Controller
{
    [HttpPost]
    [Authorize(Policy="CustomerOnly")]
    public async Task<IActionResult> CreateBooking([FromBody] BookingTourRequest request)
    {
        await Task.Delay(100);
        return Ok();
    }
}
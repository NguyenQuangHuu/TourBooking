using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLySanPham.Application.Commons;
using QuanLySanPham.Application.Features.BookingTour.Commands;
using QuanLySanPham.Domain.Aggregates.Bookings;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Presentations.DTOs.Requests;

namespace QuanLySanPham.Presentations.APIs;
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BookingsController : Controller
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    [Authorize(Policy="CustomerOnly")]
    public async Task<IActionResult> CreateBooking([FromBody] BookingTourRequest request,CancellationToken ct)
    {
        var userId = User.GetUserIdPrincipal();
        BookingTourCommand cmd = new BookingTourCommand(userId,TourInstanceId.From(request.TourInstanceId), request.TotalSlots);
        var result = await _mediator.Send(cmd, ct);
        return Ok(result);
    }
}
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLySanPham.Application.Features.Tours.Commands;
using QuanLySanPham.Application.Features.Tours.Queries;
using QuanLySanPham.Domain.Aggregates.Tours;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Presentations.DTOs.Requests;

namespace QuanLySanPham.Presentations.APIs;

/// <summary>
/// Quản lý Tour cho phía Admin
/// </summary>
/// <param name="mediator">Nhận IMediator để phát sự kiện đến các handler</param>
[ApiController]
[Route("api/[controller]")]
public class ToursController(IMediator mediator) : Controller
{
    /// <summary>
    /// Lấy toàn bộ tour master
    /// </summary>
    /// <returns>Danh sách TourMaster đã lưu trữ</returns>
    [HttpGet]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> GetTourMasters()
    {
        var query = new GetAllTourMastersQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Thêm Tour Master
    /// </summary>
    /// <param name="form">Chứa thông tin của tour master</param>
    /// <returns>Id của đối tượng vừa được thêm vào</returns>
    /// <response code="200">Trả về Id của Tour vừa thêm vào</response>
    /// <response code="400">Request không hợp lệ</response>
    [HttpPost]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> AddTourMaster([FromBody] CreateTourMasterRequest createTourMasterRequest)
    {
        List<TourMasterDestination> listDestination = new();
        foreach (var value in createTourMasterRequest.Destinations)
            listDestination.Add(new TourMasterDestination(DestinationId.From(value.DestinationId), value.Order,
                value.StayDays));
        List<TourMasterPoi> listPoi = new();
        foreach (var value in createTourMasterRequest.PointOfInterests)
            listPoi.Add(new TourMasterPoi(PointOfInterestId.From(value.PoiId), value.Order));
        var cost = new Money(createTourMasterRequest.OperatingCost);
        var duration = new TimeSchedule(createTourMasterRequest.DurationEstimate);
        var command =
            new CreateTourMasterCommand(createTourMasterRequest.Name, cost, duration, listDestination, listPoi);
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(AddTourMaster), new { tourMasterId = result });
    }

    /// <summary>
    /// Lấy đối tượng TourMaster theo Id
    /// </summary>
    /// <param name="id">Id của đối tượng</param>
    /// <returns>Đối tượng hợp lệ hoặc rỗng</returns>
    /// <response code="200">Trả về TourMaster hợp lệ</response>
    /// <response code="403">Id không hợp lệ</response>
    /// <response code="404">Không tìm thấy</response>
    /// <response code="500">Lỗi hệ thống</response>
    [HttpGet("{id}")]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> GetTourMasterById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest("Id không hợp lệ!");

        try
        {
            var queryTourMasterById = new GetTourMasterByIdQuery(id);
            var result = await mediator.Send(queryTourMasterById);
            if (result == null) return NotFound("Không tìm thấy!");
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
    }

    [HttpGet("{id}/tour-instance")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTourInstanceByTourMasterId([FromRoute] Guid id,
        CancellationToken token = default)
    {
        var tourMasterId = TourMasterId.From(id);
        var command = new GetTourInstanceByTourMasterIdQuery(tourMasterId);
        var result = await mediator.Send(command, token);
        return Ok(result);
    }

    /// <summary>
    /// Tạo Tour Instance
    /// </summary>
    /// <param name="id">Id TourMaster</param>
    /// <param name="request">Đối tượng request</param>
    /// <returns>Thành công hoặc thất bại</returns>
    [HttpPost("{id}/tour-instance")]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> CreateTourInstance([FromRoute] Guid id,
        [FromBody] CreateTourInstanceRequest request)
    {
        var operationalPeriod = new DateRange(request.StartDate, request.EndDate);
        var slotInfo = new SlotInfo(request.OpenedSlots);
        var pricePerPax = new Money(request.PricePerTax);
        var tourMasterId = TourMasterId.From(id);
        var command = new CreateTourInstanceCommand(operationalPeriod, slotInfo, pricePerPax, tourMasterId);
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(CreateTourInstance), new { tourInstanceId = result, tourMasterId = id });
    }
}
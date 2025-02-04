using BigTech.Domain.Dto.Report;
using BigTech.Domain.Interfaces.Services;
using BigTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BigTech.Api.Controllers;
//[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BaseResult<ReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResult<ReportDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> GetReport(long id)
    {
        var response = await _reportService.GetReportByIdAsync(id);
        if (response.IsSucess)
            return Ok(response);
        return BadRequest(response);
    }
}

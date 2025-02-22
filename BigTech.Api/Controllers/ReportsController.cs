using Asp.Versioning;
using BigTech.Domain.Dto.Report;
using BigTech.Domain.Interfaces.Services;
using BigTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BigTech.Api.Controllers;

//[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Получение отчётов пользователя
    /// </summary>
    /// <param name="userId"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET
    ///     {
    ///        "id": "1",
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Если отчёты были найдены</response>
    /// <response code="400">Если отчёты не были найдены</response>
    [HttpGet("getUserReports/{userId}")]
    [ProducesResponseType(typeof(BaseResult<ReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResult<ReportDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> GetUserReports(long userId)
    {
        var response = await _reportService.GetReportsAsync(userId);
        if (response.IsSuccess)
            return Ok(response);
        return BadRequest(response);
    }

    /// <summary>
    /// Получение отчёта с указанием идентификатора
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET
    ///     {
    ///        "id": "1",
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Если отчёт был найден</response>
    /// <response code="400">Если отчёт не был найден</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BaseResult<ReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResult<ReportDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> GetReport(long id)
    {
        var response = await _reportService.GetReportByIdAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        return BadRequest(response);
    }

    /// <summary>
    /// Удаление отчёта с указанием идентификатора
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE
    ///     {
    ///        "id": "1",
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Если отчёт удалился</response>
    /// <response code="400">Если отчёт не был удален</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> Delete(long id)
    {
        var response = await _reportService.DeleteReportAsync(id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    /// <summary>
    /// Создание отчёта
    /// </summary>
    /// <param name="dto"></param>
    /// <remarks>
    /// Request for create report:
    ///
    ///     POST
    ///     {
    ///        "name": "Report #1",
    ///        "description": "Test report",
    ///        "userId": 1
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Если отчёт создался</response>
    /// <response code="400">Если отчёт не был создан</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> Create([FromBody] CreateReportDto dto)
    {
        var response = await _reportService.CreateReportAsync(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    /// <summary>
    /// Обновление отчёта с указанием основных свойств
    /// </summary>
    /// <param name="dto"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT
    ///     {
    ///        "id": 1
    ///        "name": "Report #2,
    ///        "description": "Test report2",
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Если отчёт обновился</response>
    /// <response code="400">Если отчёт не был обновлен</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> Update([FromBody] UpdateReportDto dto)
    {
        var response = await _reportService.UpdateReportAsync(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}

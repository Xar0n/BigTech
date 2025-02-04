using BigTech.Domain.Dto.Report;
using BigTech.Domain.Result;

namespace BigTech.Domain.Interfaces.Services;

/// <summary>
/// Сервис отвечающий за работу с доменность частью отчета (Report)
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Получение всех отчетов пользователя
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CollectionResult<ReportDto>> GetReportsAsync(long userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение отчета по id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<BaseResult<ReportDto>> GetReportByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать отчет
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление отчета по идентификатору
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BaseResult<ReportDto>> DeleteReportAsync(long id);

    /// <summary>
    /// Удаление отчета по идентификатору
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BaseResult<ReportDto>> UpdateReportAsync(UpdateReportDto dto);
}

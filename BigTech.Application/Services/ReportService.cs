using AutoMapper;
using BigTech.Application.Resources;
using BigTech.Domain.Dto.Report;
using BigTech.Domain.Entity;
using BigTech.Domain.Enum;
using BigTech.Domain.Extensions;
using BigTech.Domain.Interfaces.Repositories;
using BigTech.Domain.Interfaces.Services;
using BigTech.Domain.Interfaces.Validations;
using BigTech.Domain.Result;
using BigTech.Domain.Settings;
using BigTech.Producer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Serilog;

namespace BigTech.Application.Services;
public class ReportService : IReportService
{
    private readonly IBaseRepository<Report> _reportRepository;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IReportValidator _reportValidator;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IMessageProducer _producer;
    private readonly IOptions<RabbitMqSettings> _rabbitMqOptions;
    private readonly IDistributedCache _distributedCache;

    public ReportService(IBaseRepository<Report> reportRepository,
        IBaseRepository<User> userRepository,
        IReportValidator reportValidator,
        IMapper mapper,
        ILogger logger,
        IMessageProducer producer,
        IOptions<RabbitMqSettings> rabbitMqOptions,
        IDistributedCache distributedCache)
    {
        _reportRepository = reportRepository;
        _userRepository = userRepository;
        _reportValidator = reportValidator;
        _mapper = mapper;
        _logger = logger;
        _producer = producer;
        _rabbitMqOptions = rabbitMqOptions;
        _distributedCache = distributedCache;
    }

    /// <inheritdoc />
    public async Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userRepository.GetAll()
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);
            var report = await _reportRepository.GetAll()
                .FirstOrDefaultAsync(r => r.Name == dto.Name);
            var result = _reportValidator.CreateValidator(report, user);
            if (!result.IsSuccess)
            {
                return new BaseResult<ReportDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode,
                };
            }
            report = new Report
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = user.Id
            };
            await _reportRepository.CreateAsync(report);
            await _reportRepository.SaveChangesAsync();
            await _producer.SendMessage(report, _rabbitMqOptions.Value.RoutingKey, _rabbitMqOptions.Value.ExchangeName);

            return new BaseResult<ReportDto>()
            {
                Data = _mapper.Map<ReportDto>(report),
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return new BaseResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    /// <inheritdoc />
    public async Task<BaseResult<ReportDto>> DeleteReportAsync(long id)
    {
        try
        {
            var report = await _reportRepository.GetAll()
                .FirstOrDefaultAsync(r => r.Id == id);
            var result = _reportValidator.ValidateOnNull(report);
            if (!result.IsSuccess)
            {
                return new BaseResult<ReportDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode,
                };
            }
            
            _reportRepository.Remove(report);
            await _reportRepository.SaveChangesAsync();
            return new BaseResult<ReportDto>()
            {
                Data = _mapper.Map<ReportDto>(report),
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return new BaseResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    /// <inheritdoc />
    public async Task<BaseResult<ReportDto>> GetReportByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        ReportDto? report;

        try
        {
            report = _reportRepository.GetAll()
                .AsEnumerable()
                .Select(r => new ReportDto(r.Id, r.Name,
                    r.Description, r.CreatedAt.ToLongDateString()))
                .FirstOrDefault(r => r.Id == id);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return new BaseResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }

        if (report == null)
        {
            _logger.Warning("Отчет с {id} не найден", id);
            return new BaseResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.ReportNotFound,
                ErrorCode = (int)ErrorCodes.ReportNotFound
            };
        }

        _distributedCache.SetObject($"Report_{id}", report);

        return new BaseResult<ReportDto>()
        {
            Data = report
        };
    }

    /// <inheritdoc />
    public async Task<CollectionResult<ReportDto>> GetReportsAsync(long userId, CancellationToken cancellationToken = default)
    {
        ReportDto[] reports;
        try
        {
            reports = await _reportRepository.GetAll()
                .Where(r => r.UserId == userId)
                .Select(r => new ReportDto(r.Id, r.Name,
                    r.Description, r.CreatedAt.ToLongDateString()))
                .ToArrayAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return new CollectionResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }

        if (!reports.Any())
        {
            _logger.Warning(ErrorMessage.ReportsNotFound);
            return new CollectionResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.ReportsNotFound,
                ErrorCode = (int)ErrorCodes.ReportsNotFound
            };
        }

        return new CollectionResult<ReportDto>()
        {
            Data = reports,
            Count = reports.Length
        };
    }

    public async Task<BaseResult<ReportDto>> UpdateReportAsync(UpdateReportDto dto)
    {
        try
        {
            var report = await _reportRepository.GetAll()
                .FirstOrDefaultAsync(r => r.Id == dto.Id);

            var result = _reportValidator.ValidateOnNull(report);
            if (!result.IsSuccess)
            {
                return new BaseResult<ReportDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode,
                };
            }

            report.Name = dto.Name;
            report.Description = dto.Description;

            _reportRepository.Update(report);
            await _reportRepository.SaveChangesAsync();
            return new BaseResult<ReportDto>()
            {
                Data = _mapper.Map<ReportDto>(report),
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return new BaseResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }
}

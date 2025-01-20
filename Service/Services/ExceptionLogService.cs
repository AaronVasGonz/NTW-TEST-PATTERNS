using Data.Repository;
using Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public interface IExceptionLogService
{
    Task<ExceptionLog> SaveAsync(ExceptionLog exceptionLog);
}

public class ExceptionLogService : IExceptionLogService
{
    private readonly IExeptionLogRepository _exeptionLogRepository;
    public ExceptionLogService(IExeptionLogRepository exeptionLogRepository)
    {
        _exeptionLogRepository = exeptionLogRepository;
    }

    public async Task<ExceptionLog> SaveAsync(ExceptionLog exceptionLog)
    {
        return await _exeptionLogRepository.SaveAsync(exceptionLog);
    }
}

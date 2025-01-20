using Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository;

public interface IExeptionLogRepository
{
    Task<ExceptionLog> SaveAsync(ExceptionLog exceptionLog);
}

public class ExeptionLogRepository : RepositoryBase<ExceptionLog>, IExeptionLogRepository
{
    public async Task<ExceptionLog> SaveAsync(ExceptionLog exceptionLog)
    {
        var extis = exceptionLog.LogId > 0;

        if (extis)
            await UpdateAsync(exceptionLog);
        else
            await CreateAsync(exceptionLog);
        var updatedExceptionLogs = await ReadAsync();
        return updatedExceptionLogs.SingleOrDefault(x => x.LogId == exceptionLog.LogId);
    }
}

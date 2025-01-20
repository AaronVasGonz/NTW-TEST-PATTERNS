using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.EFModels;
using Service.Services;
using System;

namespace Service.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IExceptionLogService _exceptionLogService;

        public GlobalExceptionFilter(IExceptionLogService exceptionLogService)
        {
            _exceptionLogService = exceptionLogService;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var exceptionLog = new ExceptionLog
            {
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Date = DateTime.Now
            };

            _exceptionLogService.SaveAsync(exceptionLog).Wait();
            _exceptionLogService.SaveAsync(exceptionLog).Wait();

            context.Result = new ObjectResult("An error occurred") { StatusCode = 500 };
            context.ExceptionHandled = true;
        }
    }
}

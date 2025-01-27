using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.EFModels;
using Service.Services;

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

            // Save the error in the logs
            _exceptionLogService.SaveAsync(exceptionLog).Wait();

            //Classify the exception and return the appropriate status code
            if (exception is ArgumentException || exception is InvalidOperationException)
            {
                // return Bad Request (400)
                context.Result = new BadRequestObjectResult(new
                {
                    Error = exception.Message,
                    Code = 400
                });
                context.HttpContext.Response.StatusCode = 400;
            }
            else if (exception is KeyNotFoundException)
            {
                //return Not Found (404)
                context.Result = new NotFoundObjectResult(new
                {
                    Error = exception.Message,
                    Code = 404
                });
                context.HttpContext.Response.StatusCode = 404;
            }
            else
            {
                // Return an Internal Server Error (500)
                context.Result = new ObjectResult(new
                {
                    Error = "An unexpected error occurred. Please try again later.",
                    Code = 500
                })
                {
                    StatusCode = 500
                };
            }

            // mark the exception as handled
            context.ExceptionHandled = true;
        }
    }
}

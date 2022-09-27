using CrossCutting.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Application.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException || context.Exception is FormatException)
                context.Result = new ObjectResult(context.Exception.Message)
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                };

            _logger.LogError(context.Exception.Message);
        }


    }

    public class ContentResult
    {
        public string Message;
        public ContentResult(string message)
        {
            Message = message;
        }
    }
}
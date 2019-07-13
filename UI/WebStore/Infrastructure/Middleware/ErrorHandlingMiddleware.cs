using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebStore.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate Next, ILogger<ErrorHandlingMiddleware> Logger)
        {
            _Next = Next;
            _logger = Logger;
        }

        public async Task Invoke(HttpContext Context)
        {
            try
            {
                await _Next(Context);
            }
            catch (Exception error)
            {
                await HandleExceptionAsync(Context, error);
                throw;
            }
        }

        private Task HandleExceptionAsync(HttpContext Context, Exception Error)
        {
            _logger.LogError(Error.Message, "В ходе обработки входящего запроса возникло исключение");
            return Task.CompletedTask;
        }
    }
}

using System.Net;
using System.Text.Json;

namespace CinemaApp.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;


        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }



        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception occurred"
                );


                await HandleExceptionAsync(
                    context,
                    ex
                );
            }
        }



        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";


            var statusCode = exception switch
            {
                ArgumentException => HttpStatusCode.BadRequest,

                InvalidOperationException => HttpStatusCode.BadRequest,

                UnauthorizedAccessException => HttpStatusCode.Unauthorized,

                KeyNotFoundException => HttpStatusCode.NotFound,

                _ => HttpStatusCode.InternalServerError
            };


            context.Response.StatusCode = (int)statusCode;



            var response = new
            {
                statusCode = context.Response.StatusCode,

                message = exception.Message,

                timestamp = DateTime.UtcNow
            };


            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    }
}
